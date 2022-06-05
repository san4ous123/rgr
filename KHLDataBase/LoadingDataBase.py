import sqlite3 as sq

def load_to_data_base(season, players):
    with sq.connect('KHL.db') as con:
        cur = con.cursor()

        season = season.replace('-','_')

        cur.execute(f'DROP TABLE IF EXISTS season_{season}')

        cur.execute(f"""CREATE TABLE IF NOT EXISTS season_{season}(
        fullname STRING PRIMARY KEY,
        season STRING,
        nation STRING,
        age INTEGER,
        position STRING,
        games_played INTEGER,
        goals_scored INTEGER,
        assists INTEGER,
        points INTEGER
        )""")

        sqliteInsertQuery = f"""INSERT OR REPLACE INTO season_{season} VALUES (?,?,?,?,?,?,?,?,?)"""

        cur.executemany(sqliteInsertQuery, players)

def load_players_seasons_to_data_base(seasons):
    with sq.connect('KHL.db') as con:
        cur = con.cursor()

        #season = season.replace('-','_')

        tableTitle = 'players_seasons'

        cur.execute(f'DROP TABLE IF EXISTS {tableTitle}')

        cur.execute(f"""CREATE TABLE IF NOT EXISTS {tableTitle}(
        id_player INTEGER,
        id_season INTEGER,
        id_nation INTEGER,
        age INTEGER,
        position INTEGER,
        games_played INTEGER,
        goals_scored INTEGER,
        assists INTEGER,
        points INTEGER,
        PRIMARY KEY (id_player, id_season),
        FOREIGN KEY (id_nation) REFERENCES nations(id)
        )""")

        for season in seasons:
            cur.execute(f"""
            INSERT INTO {tableTitle} (id_player, id_season, id_nation, age, position, games_played, goals_scored, assists, points)
            SELECT players.id as id_player, seasons.id as id_nation, nations.id as id_nation, age, position, games_played, goals_scored, assists, points
            FROM players
            INNER JOIN nations
            INNER JOIN seasons
            INNER JOIN season_202122
            ON season_202122.fullname = players.fullname AND season_202122.season = seasons.title AND season_202122.nation = nations.title
            """)