from PyQt5 import QtCore, QtGui, QtWidgets
from FileTypeSelector import Ui_FileTypeSelector


class MainFormDrop (QtGui.QDropEvent):
    def dropAction(cls, self):
        FileTypeSelector = QtWidgets.QDialog()
        ui = Ui_FileTypeSelector()
        ui.setupUi(FileTypeSelector)
        FileTypeSelector.show()
        return super().dropAction(self)
