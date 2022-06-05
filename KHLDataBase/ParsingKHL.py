import requests
from bs4 import BeautifulSoup

def parsing_players_season(season, amountPages):
    players = []

    for numPage in range(1, amountPages + 1): # amountPages + 1
        pageSeason = requests.get('https://www.quanthockey.com/scripts/AjaxPaginate.php?cat=Season&pos=Players&SS=' + season + '&af=0&nat=' + season + '&st=reg&sort=P&so=DESC&page=' + str(numPage) + '&league=KHL&lang=en&rnd=&dt=1&sd=undefined&ed=undefined')

        soup = BeautifulSoup(pageSeason.text, 'html.parser')

        table = soup.find('table', id='statistics').find('tbody')

        namesPlayers = table.select('th:nth-child(3)')
        srcNationsPlayers = table.find_all('img')
        agesPlayers = table.select('td:nth-child(4)')
        positionsPlayers = table.select('td:nth-child(5)')
        gamesPlayedPlayers = table.select('td:nth-child(6)')
        goalsScoredPlayers = table.select('td:nth-child(7)')
        assistsPlayers = table.select('td:nth-child(8)')
        points = table.select('td:nth-child(9)')

        nationsPlayers = handle_src_nation_players(srcNationsPlayers)

        for i in range(len(namesPlayers)):
            players.append((namesPlayers[i].text, season, nationsPlayers[i], agesPlayers[i].text, positionsPlayers[i].text, gamesPlayedPlayers[i].text, goalsScoredPlayers[i].text, assistsPlayers[i].text, points[i].text))

    return players

def find_amount_pages(season):
    amount = 0

    pageSeason = requests.get('https://www.quanthockey.com/scripts/AjaxPaginate.php?cat=Season&pos=Players&SS=' + season + '&af=0&nat=' + season + '&st=reg&sort=P&so=DESC&page=1&league=KHL&lang=en&rnd=&dt=1&sd=undefined&ed=undefined')
    soup = BeautifulSoup(pageSeason.text, 'html.parser')

    tableTop = soup.find('div', class_='tabletop')

    amountPages = tableTop.select('li:nth-last-child(2)')

    return int(amountPages[0].text)


def handle_src_nation_players(srcNationsPlayers):
    nationsPlayers = []

    for item in srcNationsPlayers:
        temp = item.get('src')

        temp = temp.replace('https://cdn77.quanthockey.com/img/country-flags/','')
        temp = temp.replace('-Flag-16.png','')

        nationsPlayers.append(temp)

    return nationsPlayers

class player(object):
    def __init__(self, season = None, fullName = None, nation = None, age = None, position = None, gamesPlayed = None, goalsScored = None, assists = None, points = None):
        self.season = season  
        self.fullName = fullName
        self.nation = nation
        self.age = int(age)
        self.position = position
        self.gamesPlayed = int(gamesPlayed)
        self.goalsScored = int(goalsScored)
        self.assists = int(assists)
        self.points = int(points)

    def showInfo(self):
        print(self.season, self.fullName, self.nation, self.age, self.position, self.gamesPlayed, self.goalsScored, self.assists, self.points)
