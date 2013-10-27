#ifndef RULE_H
#define RULE_H

#include <QObject>
#include <QList>
#include "condition.h"

class Rule : public QObject
{
    Q_OBJECT

private:
    QList<Condition> conditions;

public:
    explicit Rule();

    bool apply();

    Condition addCondition(const Condition &condition);

signals:

public slots:

};

#endif // RULE_H
