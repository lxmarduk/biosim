#include "qcell.h"


int QCell::getCellID() const
{
    return cellID;
}

void QCell::setCellID(int value)
{
    cellID = value;
}
QCell::QCell(int id) :
    QObject(NULL)
{
    this->cellID = id;
}

QCell::~QCell()
{

}
