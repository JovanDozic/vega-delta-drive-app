import csv

def convert_csv_to_sql(csv_filepath, sql_filepath1, sql_filepath2):
    sql_insert_template = "INSERT INTO Drivers (Brand, FirstName, LastName, Latitude, Longitude, StartPrice, PricePerKm, IsBooked, Rating) VALUES (%s, %s, %s, %s, %s, %s, %s, NULL, NULL);"

    with open(csv_filepath, 'r', newline='', encoding='utf-8') as csv_file:
        reader = csv.reader(csv_file)
        next(reader)
        rows = [row for row in reader if row]

    # Removing `,` from the last column
    for i in range(len(rows)):
        if not rows[i][-1]:
            rows[i] = rows[i][:-1]

    half_point = len(rows) // 2

    with open(sql_filepath1, 'w', newline='', encoding='utf-8') as sql_file1:
        for row in rows[:half_point]:
            write_sql_insert(row, sql_file1, sql_insert_template)

    with open(sql_filepath2, 'w', newline='', encoding='utf-8') as sql_file2:
        for row in rows[half_point:]:
            write_sql_insert(row, sql_file2, sql_insert_template)

def write_sql_insert(row, sql_file, template):
    Brand, FirstName, LastName, Latitude, Longitude, StartPrice, PricePerKm = row
    StartPrice = StartPrice.replace('EUR', '').strip()
    PricePerKm = PricePerKm.replace('EUR', '').strip()
    Latitude = float(Latitude)
    Longitude = float(Longitude)

    sql_file.write(template % (
        f'"{Brand}"', f'"{FirstName}"', f'"{LastName}"',
        Latitude, Longitude,
        f'"{StartPrice}"', f'"{PricePerKm}"'
    ) + '\n')

if __name__ == "__main__":
    convert_csv_to_sql('delta-drive.csv', 'output1.sql', 'output2.sql')
