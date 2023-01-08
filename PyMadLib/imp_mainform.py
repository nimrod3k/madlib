
from PyQt5 import QtCore, QtGui, QtWidgets
from ui_mainform import Ui_MainWindow
from imp_madlibeditor import MadEditor
from MadLib import MadLib

madLib = MadLib()

class mainform(Ui_MainWindow) :
    
    def __init__(self, *args, **kwargs):
        return super().__init__(*args, **kwargs)

    def setupUi(self, MainWindow):
        ret = super().setupUi(MainWindow)
        self.madlib_editor_btn.clicked.connect(self.buttonCreate_Click)
        return ret

    def buttonCreate_Click(self, checked) :
        madEditor = QtWidgets.QDialog()
        dialogUI = MadEditor()
        dialogUI.setupUi(madEditor)
        if madEditor.exec() is 1 :
            madLib.create_madlib(dialogUI.getText())
        else :
            return
        

