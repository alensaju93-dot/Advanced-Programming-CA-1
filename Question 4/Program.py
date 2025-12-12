import csv
from bs4 import BeautifulSoup

sources = {
    "resort1.html": [],
    "resort2.html": []
}

output_rows = []

for file_name in sources:
    with open(file_name, "r", encoding="utf-8") as f:
        dom = BeautifulSoup(f.read(), "html.parser")

    hotel_title = dom.select_one("header > h1").text.strip()
    room_blocks = dom.select("article.room-item")

    for block in room_blocks:
        room_label = block.find("span", class_="label").text.strip()
        rate = block.find("span", class_="rate").text.strip()
        output_rows.append([hotel_title, room_label, rate])

with open("room_prices_v3.csv", "w", newline="", encoding="utf-8") as file:
    writer = csv.writer(file)
    writer.writerow(["Hotel_Name", "Room_Category", "Price_Per_Night"])
    writer.writerows(output_rows)

with open("room_prices_v3.csv", "r", encoding="utf-8") as file:
    for line in file:
        print(line.strip())