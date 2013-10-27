#include "condition.h"

Condition::Condition(const QString &name)
{
    this->conditionName = QString(name);
}

Condition::~Condition()
{
}

bool Condition::check()
{
    return false;
}

