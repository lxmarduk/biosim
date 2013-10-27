#ifndef RULE_H
#define RULE_H

#include <QList>
#include "condition.h"

class Rule
{
private:
    QList<Condition> conditions;

public:
    explicit Rule();
    virtual ~Rule();

    virtual bool apply();

    void addCondition(const Condition &condition);
};

#endif // RULE_H
