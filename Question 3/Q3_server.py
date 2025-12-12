import socket
import threading
import json
import sqlite3
import random
import string
from datetime import datetime

SERVER_IP = "127.0.0.1"
SERVER_PORT = 65432
DB_NAME = "student_records.db"

def prepare_storage():
    db = sqlite3.connect(DB_NAME)
    cur = db.cursor()
    cur.execute("""
        CREATE TABLE IF NOT EXISTS submissions (
            record_id INTEGER PRIMARY KEY AUTOINCREMENT,
            reference TEXT UNIQUE,
            applicant TEXT,
            location TEXT,
            education TEXT,
            programme TEXT,
            year INTEGER,
            month INTEGER,
            timestamp TEXT
        )
    """)
    db.commit()
    db.close()

def create_reference():
    rand = ''.join(random.sample(string.ascii_uppercase + string.digits, 6))
    date = datetime.utcnow().strftime("%y%m%d")
    return f"REF-{date}-{rand}"

def receive_payload(sock):
    buffer = b""
    while True:
        chunk = sock.recv(2048)
        if not chunk:
            break
        buffer += chunk
        if b"\n" in chunk:
            break
    return buffer.decode().strip()

def store_application(data):
    ref = create_reference()
    db = sqlite3.connect(DB_NAME)
    cur = db.cursor()
    cur.execute("""
        INSERT INTO submissions
        (reference, applicant, location, education, programme, year, month, timestamp)
        VALUES (?, ?, ?, ?, ?, ?, ?, ?)
    """, (
        ref,
        data["full_name"],
        data["address"],
        data["education"],
        data["course"],
        data["year"],
        data["month"],
        datetime.utcnow().isoformat()
    ))
    db.commit()
    db.close()
    return ref

def client_handler(client_socket):
    try:
        raw = receive_payload(client_socket)
        info = json.loads(raw)

        if not info.get("full_name") or not info.get("course"):
            raise ValueError("Missing fields")

        if info["month"] not in range(1, 13) or info["year"] < 2024:
            raise ValueError("Invalid intake")

        ref_code = store_application(info)
        response = {"result": "accepted", "ref": ref_code}
    except Exception:
        response = {"result": "rejected", "reason": "Invalid submission"}
    finally:
        client_socket.sendall((json.dumps(response) + "\n").encode())
        client_socket.close()

def launch_server():
    prepare_storage()
    srv = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    srv.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
    srv.bind((SERVER_IP, SERVER_PORT))
    srv.listen()
    print(f"Server active on {SERVER_IP}:{SERVER_PORT}")

    try:
        while True:
            client, _ = srv.accept()
            threading.Thread(
                target=client_handler,
                args=(client,),
                daemon=True
            ).start()
    except KeyboardInterrupt:
        print("\nServer shutdown.")
    finally:
        srv.close()

if __name__ == "__main__":
    launch_server()