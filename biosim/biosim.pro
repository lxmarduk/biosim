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
    condition.cpp \
    aboutdialog.cpp

HEADERS  += mainwindow.h \
    qcell.h \
    rule.h \
    condition.h \
    aboutdialog.h

FORMS    += mainwindow.ui \
    aboutdialog.ui

RESOURCES += \
    Resources.qrc
