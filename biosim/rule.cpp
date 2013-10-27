#include "rule.h"

Rule::Rule()
{
    conditions = QList<Condition>();
}

Rule::~Rule()
{
    conditions.clear();
}

bool Rule::apply(QCell &cell) {
    cell;
    return true;
}

void Rule::addCondition(const Condition &condition) {
    conditions.append(condition);
}
