#include "mainwindow.h"
#include "ui_mainwindow.h"
#include "aboutdialog.h"

MainWindow::MainWindow(QWidget *parent) :
    QMainWindow(parent),
    ui(new Ui::MainWindow)
{
    ui->setupUi(this);
}

MainWindow::~MainWindow()
{
    delete ui;
}

void MainWindow::on_actionExit_triggered()
{
    this->close();
}

void MainWindow::on_actionAbout_Biosim_triggered()
{
    /*
    QMessageBox about;

    about.setText("Biosim v1.0");
    about.setInformativeText("(c) 2013\nAlex Chaban (lxmarduk)");
    about.setStandardButtons(QMessageBox::Ok);
    about.exec();//*/
    AboutDialog* about = new AboutDialog(this);
    about->setModal(true);
    about->show();
}
