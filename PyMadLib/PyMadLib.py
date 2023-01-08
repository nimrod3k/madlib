import sys
from os import listdir
from os.path import isfile, join

from ui_mainform import Ui_MainWindow
from imp_mainform import mainform
from PyQt5 import QtCore, QtGui, QtWidgets



if __name__ == "__main__":
    import sys
    app = QtWidgets.QApplication(sys.argv)
    ui_mainWindow = QtWidgets.QMainWindow()
    ui = mainform()
    ui.setupUi(ui_mainWindow)
    ui_mainWindow.show()
    sys.exit(app.exec_())
