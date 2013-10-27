#ifndef QCELL_H
#define QCELL_H

#include <QObject>
#include "rule.h"
#include "utils.h"

class QCell : public QObject
{
    Q_OBJECT

private:
    int cellID;

    bool isLive;
    bool mustExplore;
    bool feeding;
    bool evade;
    bool chase;

    double angerLevel;
    double hungerLevel;
    double healthLevel;

public:
    explicit QCell(int id);
    virtual ~QCell();

    int getCellID() const;
    void setCellID(int value);

    void processRule(Rule &rule);
    void processRules(QList<Rule> rules);

    bool isAlive() const;
    void setAlive(bool alive);
    bool isExploring() const;
    void setExplore(bool explore);
    bool isFeeding() const;
    void setFeeding(bool feeding);
    bool isEvade() const;
    void setEvade(bool evade);
    double getAngerLevel() const;
    void setAngerLevel(double newLevel);
    void provoke(double amount);
    void calm(double amount);
    double getHungerLevel() const;
    void setHungerLevel(double newLevel);
    void hunger(double amount);
    void feed(double amount);
    double getHealthLevel() const;
    void setHealthLevel(double newLevel);
    void injure(double amount);
    void heal(double amount);
signals:

public slots:

};

#endif // QCELL_H
