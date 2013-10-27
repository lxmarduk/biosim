#ifndef CONDITION_H
#define CONDITION_H

#include <QObject>

class Condition : public QObject
{
    Q_OBJECT

private:
    QString conditionName;

public:
    explicit Condition(const QString &name);

    virtual bool check() = NULL;

signals:

public slots:

};

#endif // CONDITION_H
