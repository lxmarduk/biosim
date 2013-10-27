#ifndef CONDITION_H
#define CONDITION_H

#include <QString>

class Condition
{

private:
    QString conditionName;

public:
    explicit Condition(const QString &name);
    virtual ~Condition();

    virtual bool check();
};

#endif // CONDITION_H
