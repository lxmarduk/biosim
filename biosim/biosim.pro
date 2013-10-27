#-------------------------------------------------
#
# Project created by QtCreator 2013-10-27T10:59:25
#
#-------------------------------------------------

QT       += core gui

greaterThan(QT_MAJOR_VERSION, 4): QT += widgets

TARGET = biosim
TEMPLATE = app


SOURCES += main.cpp\
        mainwindow.cpp \
    qcell.cpp \
    rule.cpp \
    condition.cpp

HEADERS  += mainwindow.h \
    qcell.h \
    rule.h \
    condition.h

FORMS    += mainwindow.ui

RESOURCES += \
    Resources.qrc
