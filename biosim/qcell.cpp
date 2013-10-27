#include "qcell.h"


int QCell::getCellID() const
{
    return cellID;
}

void QCell::setCellID(int value)
{
    cellID = value;
}

void QCell::processRule(Rule &rule)
{
    if(rule.apply(*this)) {
        return;
    }
}

void QCell::processRules(QList<Rule> rules)
{
    int len = rules.count();
    for(int i = 0; i < len; ++i) {
        rules[i].apply(*this);
    }
}

bool QCell::isAlive() const
{
    return isLive;
}

void QCell::setAlive(bool alive)
{
    isLive = alive;
}

bool QCell::isExploring() const
{
    return mustExplore;
}

void QCell::setExplore(bool explore)
{
    mustExplore = explore;
}

bool QCell::isFeeding() const
{
    return feeding;
}

void QCell::setFeeding(bool feeding)
{
    this->feeding = feeding;
}

bool QCell::isEvade() const
{
    return evade;
}

void QCell::setEvade(bool evade)
{
    this->evade = evade;
}

double QCell::getAngerLevel() const
{
    return angerLevel;
}

void QCell::setAngerLevel(double newLevel)
{
    angerLevel = CLAMP(0.0, 100.0, newLevel);
}

void QCell::provoke(double amount)
{
    angerLevel += amount;
    angerLevel = MIN(100.0, angerLevel);
}

void QCell::calm(double amount)
{
    angerLevel -= amount;
    angerLevel = MAX(0.0, angerLevel);
}

double QCell::getHungerLevel() const
{
    return hungerLevel;
}

void QCell::setHungerLevel(double newLevel)
{
    hungerLevel = CLAMP(0.0, 100.0, newLevel);
}

void QCell::hunger(double amount)
{
    hungerLevel -= amount;
    hungerLevel = MAX(0.0, hungerLevel);
}

void QCell::feed(double amount)
{
    hungerLevel += amount;
    hungerLevel = MIN(100.0, hungerLevel);
}

double QCell::getHealthLevel() const
{
    return healthLevel;
}

void QCell::setHealthLevel(double newLevel)
{
    healthLevel = CLAMP(0.0, 100.0, newLevel);
}

void QCell::injure(double amount)
{
    healthLevel -= amount;
    healthLevel = MAX(0.0, healthLevel);
}

void QCell::heal(double amount)
{
    healthLevel += amount;
    healthLevel = MIN(100.0, healthLevel);
}
QCell::QCell(int id) :
    QObject(NULL)
{
    this->cellID = id;
}

QCell::~QCell()
{

}
