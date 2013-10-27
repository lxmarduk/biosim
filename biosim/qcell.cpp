#include "qcell.h"


int QCell::getCellID() const
{
    return cellID;
}

void QCell::setCellID(int value)
{
    cellID = value;
}

void QCell::processRule(Rule &rule)
{
    if(rule.apply(*this)) {
        return;
    }
}

void QCell::processRules(QList<Rule> rules)
{
    int len = rules.count();
    for(int i = 0; i < len; ++i) {
        rules[i].apply(*this);
    }
}
QCell::QCell(int id) :
    QObject(NULL)
{
    this->cellID = id;
}

QCell::~QCell()
{

}
