import sqlite3

def reseed_database():
    # Connect to the SQLite database
    conn = sqlite3.connect('MyWebApp.db')
    cursor = conn.cursor()

    # Drop the Students table if it exists
    cursor.execute("DROP TABLE IF EXISTS Students")

    # Create the Students table
    cursor.execute('''
        CREATE TABLE Students (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Name TEXT NOT NULL,
            Password TEXT NOT NULL,
            GPA REAL NOT NULL
        )
    ''')

    # Insert sample data
    students = [
        ('Alice Johnson', 'password123', 3.5),
        ('Bob Smith', 'password123', 3.8),
        ('Charlie Brown', 'password123', 3.2)
    ]

    cursor.executemany('''
        INSERT INTO Students (Name, Password, GPA)
        VALUES (?, ?, ?)
    ''', students)

    # Commit the transaction and close the connection
    conn.commit()
    conn.close()
    print("Database reseeded successfully.")

if __name__ == "__main__":
    reseed_database()
