import sqlite3 as sq

import ParsingKHL as pars
import LoadingDataBase as ld

def parsing_load_players_seasons_to_database(seasons):  
    for numSeason in range(len(seasons)): # len(seasons)
        amountPages = pars.find_amount_pages(seasons[numSeason])
        
        players = pars.parsing_players_season(seasons[numSeason], amountPages)

        ld.load_to_data_base(seasons[numSeason], players)

        print(f'Season {seasons[numSeason]} parsed')


def main():
    seasons = ['202122', '2020-21', '2019-20', '2018-19', '2017-18', '2016-17', '2015-16', '2014-15', '2013-14', '2012-13', '2011-12', '2010-11', '2009-10', '2008-09']

    ld.load_players_seasons_to_data_base(seasons)

    #parsing_load_players_seasons_to_database(seasons)

if __name__ == '__main__':
    main()
