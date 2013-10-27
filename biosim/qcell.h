#ifndef QCELL_H
#define QCELL_H

#include <QObject>

class QCell : public QObject
{
    Q_OBJECT

private:
    int cellID;


public:
    explicit QCell(int id);

    int getCellID() const;
    void setCellID(int value);

signals:

public slots:

};

#endif // QCELL_H
