#ifndef RULE_H
#define RULE_H

#include <QList>
#include "condition.h"


class QCell;

class Rule
{
private:
    QList<Condition> conditions;

public:
    explicit Rule();
    virtual ~Rule();

    virtual bool apply(QCell &cell);

    void addCondition(const Condition &condition);
};

#endif // RULE_H
