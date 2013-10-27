#include "condition.h"

Condition::Condition(const QString &name) :
    QObject(NULL)
{
    this->conditionName = QString(name);
}
