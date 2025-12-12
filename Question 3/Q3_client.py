import socket
import json

SERVER = "127.0.0.1"
PORT = 65432

def ask(prompt):
    while True:
        v = input(prompt).strip()
        if v:
            return v
        print("Input required.")

def ask_number(prompt):
    while True:
        try:
            return int(input(prompt))
        except ValueError:
            print("Enter numeric value.")

def main():
    print("=== Student Admission Interface ===")

    name = ask("Applicant name: ")
    address = ask("Current address: ")
    education = ask("Highest qualification: ")

    print("Programme options:")
    print("1. MSc Cyber Security")
    print("2. MSc Information Systems & Computing")
    print("3. MSc Data Analytics")

    options = {
        "1": "MSc Cyber Security",
        "2": "MSc Information Systems & Computing",
        "3": "MSc Data Analytics"
    }

    while True:
        sel = input("Select option (1-3): ").strip()
        if sel in options:
            course = options[sel]
            break
        print("Invalid selection.")

    year = ask_number("Planned intake year: ")
    month = ask_number("Planned intake month (1-12): ")
    if month < 1 or month > 12:
        month = 1

    packet = {
        "full_name": name,
        "address": address,
        "education": education,
        "course": course,
        "year": year,
        "month": month
    }

    try:
        sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        sock.connect((SERVER, PORT))
        sock.sendall((json.dumps(packet) + "\n").encode())

        reply = b""
        while True:
            part = sock.recv(2048)
            if not part:
                break
            reply += part
            if b"\n" in part:
                break

        result = json.loads(reply.decode().strip())

        if result.get("result") == "accepted":
            print("Application accepted.")
            print("Reference number:", result.get("ref"))
        else:
            print("Submission rejected.")

    except ConnectionRefusedError:
        print("Server unavailable. Please start app_server_v3.py first.")
    finally:
        sock.close()

if __name__ == "__main__":
    main()