#include "rule.h"

Rule::Rule() :
    QObject(NULL)
{
    conditions = QList<Condition>();
}

bool Rule::apply() {
    return true;
}

Condition Rule::addCondition(const Condition &condition) {
    conditions.append(condition);
}

