#ifndef QCELL_H
#define QCELL_H

#include <QObject>
#include "rule.h"

class QCell : public QObject
{
    Q_OBJECT

private:
    int cellID;


public:
    explicit QCell(int id);
    virtual ~QCell();

    int getCellID() const;
    void setCellID(int value);

    void processRule(Rule &rule);
    void processRules(QList<Rule> rules);
signals:

public slots:

};

#endif // QCELL_H
