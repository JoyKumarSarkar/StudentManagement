/* eslint-disable react/jsx-no-literals */
import DeleteForeverIcon from "@mui/icons-material/DeleteForever";
import DoneIcon from '@mui/icons-material/Done';
import EditIcon from "@mui/icons-material/Edit";
import { Backdrop, Box, Button, Checkbox, FormControlLabel, Grid, IconButton, TextField } from "@mui/material";
import { DataGridPro, GridColDef, GridRenderCellParams, } from "@mui/x-data-grid-pro";
import React from "react";
import { connect, ConnectedProps } from "react-redux";
import { setStateVariable, updateStateVariables } from "../../AppAction";
import ConfirmationPopup from "../CommonPopup/ConfirmationPopup";
import ErrorPopup from "../CommonPopup/ErrorPopup";
import SuccessPopup from "../CommonPopup/SuccessPopup";
import WarningPopup from "../CommonPopup/WarningPopup";
import "../Student/Student.css";
import { downloadStudentFile, getStudentDetails, getStudentList, updateStudent, uploadDocument } from "./StudentAction";
import { STUDENT_POPUP_ADD, STUDENT_POPUP_CLOSE, STUDENT_POPUP_DELETE, STUDENT_POPUP_EDIT, STUDENT_SETSTATE } from "./StudentActionType";
import StudentPopup from "./StudentPopup";
import CircularProgress from '@mui/material/CircularProgress';

class StudentPage extends React.Component<Props> {

  componentDidMount(): void {
    const { getStudentList } = this.props;
    const params = {
      "searchText": "",
      "showInactive": false,
    }
    getStudentList(params).then(() => {
      const { student } = this.props;
      console.log("Student:  ", student);
    })
  }

  shouldComponentUpdate() {
    return true;
  }

  generateStudentCode = (studentList: any[]) => {
    if (studentList.length === 0) return "S00001";
    const lastStudent = studentList[studentList.length - 1];
    const lastCode = parseInt(lastStudent.code.replace("S", ""), 10);
    return `S${(lastCode + 1).toString().padStart(5, "0")}`;
  };

  handleStudentAddBtn = () => {
    const { updateStateVariables, getStudentDetails } = this.props;
    const params = {
      "id": 0,
    }
    getStudentDetails(params).then(() => {
      const { student } = this.props;
      const code = this.generateStudentCode(student.allStudentList || []);
      updateStateVariables(STUDENT_POPUP_ADD, { code });
    })
  };

  handleEditStudent = (data: any) => {
    const { updateStateVariables, getStudentDetails } = this.props;
    const params = {
      "id": data.studentId,
    }
    getStudentDetails(params).then(() => {
      updateStateVariables(STUDENT_POPUP_EDIT, data);
    })
  };

  handleDeleteStudent = (data: any) => {
    const { updateStateVariables } = this.props;
    updateStateVariables(STUDENT_POPUP_DELETE, data);
  };

  handleTxtChange = (e: any) => {
    const { setStateVariable } = this.props;
    setStateVariable(STUDENT_SETSTATE, e.target.name, e.target.type === "checkbox" ? e.target.checked : e.target.value);

    if (e.target.name === "dob") {
      this.calculateAge(e.target.value);
    }

    if (e.target.name === "stateId") {
      setStateVariable(STUDENT_SETSTATE, "cityId", "");
    }
  };

  calculateAge = (dob: string) => {
    const { setStateVariable } = this.props;

    if (!dob) {
      setStateVariable(STUDENT_SETSTATE, "age", "");
      return;
    }

    const birthDate = new Date(dob);
    const today = new Date();
    let age = today.getFullYear() - birthDate.getFullYear();
    const monthDiff = today.getMonth() - birthDate.getMonth();

    if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < birthDate.getDate())) {
      age--;
    }

    setStateVariable(STUDENT_SETSTATE, "age", age.toString());
  };

  handleErrorPopupOkBtn = () => {
    const { setStateVariable } = this.props;
    setStateVariable(STUDENT_SETSTATE, "isErrorPopupOpen", false);
  };

  handleSuccessPopupOkBtn = () => {
    const { setStateVariable } = this.props;
    setStateVariable(STUDENT_SETSTATE, "isSuccessPopupOpen", false);
  };

  handleWarningPopupOkBtn = () => {
    const { setStateVariable } = this.props;
    setStateVariable(STUDENT_SETSTATE, "isWarningPopupOpen", false);
  };

  handleConfirmationPopupNoBtn = () => {
    const { setStateVariable } = this.props;
    setStateVariable(STUDENT_SETSTATE, "isConfirmationPopupOpen", false);
  };

  handleConfirmationPopupYesBtn = () => {
    const { student, updateStudent } = this.props;

    const deleteParams = {
      indicator: "D",
      studentId: student.studentId,
    };

    updateStudent(deleteParams).then(() => {
      const { student, getStudentList } = this.props;

      if (student.responseIsSuccess) {
        const params = {
          searchText: "",
          showInactive: student.showInactive,
        };
        getStudentList(params);
      }
    });
  }

  handleStudentPopupCloseBtn = () => {
    const { updateStateVariables } = this.props;
    updateStateVariables(STUDENT_POPUP_CLOSE);
  };

  handleStudentPopupSaveBtn = () => {
    const { student, updateStudent, getStudentList } = this.props;

    const subjectWiseMarksList = (student.subjectMarksList || []).map((sm: any) => ({
      marksId: sm.marksId ?? 0,
      subjectId: sm.subjectId,
      subjectName: sm.subjectName,
      marks: sm.marks,
      indicator: sm.indicator ?? (student.indicator === 'I' ? 'I' : 'U'),
    }));

    const documentList = (student.uploadedDocuments || []).map((doc: any) => {
      const baseDoc = {
        documentId: doc.documentId ?? 0,
        documentName: doc.documentName,
        documentType: doc.documentType,
        originalFileName: doc.originalFileName,
        fileName: doc.fileName,
        documentSize: doc.documentSize,
        indicator: doc.indicator,
      };

      if (doc.indicator === "U") {
        return { ...baseDoc, previousFileName: doc.previousFileName || "" };
      } else {
        return baseDoc;
      }
    });

    const payload = {
      indicator: student.indicator,
      studentId: student.indicator === 'I' ? 0 : student.studentId,
      firstName: student.firstName,
      lastName: student.lastName,
      mobile: student.mobile,
      email: student.email,
      stateId: student.stateId,
      cityId: student.cityId,
      dob: student.dob,
      isActive: student.isActive,
      subjectWiseMarks: subjectWiseMarksList,
      studentDocuments: documentList,
    };

    updateStudent(payload).then(() => {
      const { student } = this.props;
      if (student.responseIsSuccess) {
        const params = {
          searchText: '',
          showInactive: student.showInactive,
        };
        getStudentList(params);
      }
    });
  };

  handleSearchChange = (e: any) => {
    const { student, setStateVariable, getStudentList } = this.props;
    const searchText = e.target.value;
    setStateVariable(STUDENT_SETSTATE, "searchText", searchText).then(() => {
      const params = {
        searchText: searchText,
        showInactive: student.showInactive,
      };
      getStudentList(params);
    });
  };

  handleShowInactive = (e: any) => {
    const { setStateVariable, getStudentList } = this.props;
    const showInactive = e.target.checked;
    setStateVariable(STUDENT_SETSTATE, "showInactive", showInactive).then(() => {
      const params = {
        searchText: "",
        showInactive: showInactive,
      };
      getStudentList(params);
    });
  }

  handleAddSubject = () => {
    const { student, setStateVariable } = this.props;
    const { subjectId, marks, allSubjectWiseMarksList, subjectMarksList } = student;

    if (!subjectId || marks === "") {
      alert("Please select subject and enter marks.");
      return;
    }

    const subject = allSubjectWiseMarksList.find((s: any) => s.subjectId === subjectId);
    const newSubject = {
      marksId: new Date().getTime(),
      subjectId,
      subjectName: subject.subjectName,
      marks,
      indicator: "I",
    };

    const updatedList = [...subjectMarksList, newSubject];
    setStateVariable(STUDENT_SETSTATE, "subjectMarksList", updatedList);

    this.resetSubjectForm(setStateVariable);
  };

  handleEditSubject = (subject: any) => {
    const { setStateVariable } = this.props;
    setStateVariable(STUDENT_SETSTATE, "subjectId", subject.subjectId);
    setStateVariable(STUDENT_SETSTATE, "marks", subject.marks);
    setStateVariable(STUDENT_SETSTATE, "editingSubject", subject);
  };

  handleUpdateSubject = () => {
    const { student, setStateVariable } = this.props;
    const { subjectId, marks, subjectMarksList, editingSubject, allSubjectWiseMarksList } = student;

    const updatedList = subjectMarksList.map((s: any) =>
      s.marksId === editingSubject.marksId
        ? {
          ...s,
          subjectId,
          subjectName: allSubjectWiseMarksList.find((sub: any) => sub.subjectId === subjectId)?.subjectName || "",
          marks,
        }
        : s
    );

    setStateVariable(STUDENT_SETSTATE, "subjectMarksList", updatedList);
    this.resetSubjectForm(setStateVariable);
  };

  handleCancelEditSubject = () => {
    const { setStateVariable } = this.props;
    this.resetSubjectForm(setStateVariable);
  };

  handleDeleteSubject = (subjectToDelete: any) => {
    const { student, setStateVariable } = this.props;

    let updatedList;

    if (subjectToDelete.indicator === "I") {
      updatedList = student.subjectMarksList.filter(
        (s: any) => s.marksId !== subjectToDelete.marksId
      );
    } else {
      updatedList = student.subjectMarksList.map((s: any) =>
        s.marksId === subjectToDelete.marksId
          ? { ...s, indicator: "D" }
          : s
      );
    }

    setStateVariable(STUDENT_SETSTATE, "subjectMarksList", updatedList);
  };


  resetSubjectForm = (setStateVariable: any) => {
    setStateVariable(STUDENT_SETSTATE, "subjectId", "");
    setStateVariable(STUDENT_SETSTATE, "marks", "");
    setStateVariable(STUDENT_SETSTATE, "editingSubject", null);
  };

  handleAddDocument = () => {
    const { student, uploadDocument, setStateVariable } = this.props;
    const {
      documentName,
      documentType,
      uploadData,
      responseFileName,
      editingDocument,
      uploadedDocuments,
      previousFileName,
    } = student;

    if (!documentName || !documentType || !uploadData) {
      alert("Please fill all fields and upload a file.");
      return;
    }

    const formData = new FormData();
    const file = uploadData.get("file");
    formData.append("file", file);

    if (editingDocument) {
      formData.append("indicator", "U");
      formData.append("fileName", responseFileName);
      formData.append("previousFileName", previousFileName || "");

      uploadDocument(formData).then(() => {
        const { student } = this.props;
        if (student.responseIsSuccess) {
          const updatedDocs = (uploadedDocuments || []).map((doc: any) =>
            doc.documentId === editingDocument.documentId
              ? {
                ...doc,
                documentName,
                documentType,
                fileName: student.responseFileName,
                originalFileName: student.originalFileName,
                previousFileName: student.previousFileName,
                documentSize: student.documentSize,
                indicator: doc.indicator === "I" ? "I" : "U",
              }
              : doc
          );

          setStateVariable("STUDENT_SETSTATE", "uploadedDocuments", updatedDocs);
          this.resetDocumentForm(setStateVariable);
          //
          setStateVariable("STUDENT_SETSTATE", "isDocumentModalOpen", false);
        }
      });
    } else {
      uploadDocument(formData).then(() => {
        const { student } = this.props;
        const { responseFileName } = student;

        if (student.responseIsSuccess) {
          const newDoc = {
            documentId: new Date().getTime(),
            documentName,
            documentType,
            fileName: responseFileName,
            originalFileName: student.originalFileName,
            documentSize: student.documentSize,
            indicator: "I",
          };
          const updatedDocs = [...(uploadedDocuments || []), newDoc];
          setStateVariable("STUDENT_SETSTATE", "uploadedDocuments", updatedDocs);
          this.resetDocumentForm(setStateVariable);
          setStateVariable("STUDENT_SETSTATE", "isDocumentModalOpen", false);
        }
      });
    }
  };


  handleEditDocument = (docToEdit: any) => {
    const { setStateVariable } = this.props;

    setStateVariable("STUDENT_SETSTATE", "documentName", docToEdit.documentName);
    setStateVariable("STUDENT_SETSTATE", "documentType", docToEdit.documentType);
    setStateVariable("STUDENT_SETSTATE", "responseFileName", docToEdit.fileName);
    setStateVariable("STUDENT_SETSTATE", "originalFileName", docToEdit.originalFileName || docToEdit.fileName);
    setStateVariable("STUDENT_SETSTATE", "previousFileName", docToEdit.fileName);
    setStateVariable("STUDENT_SETSTATE", "editingDocument", docToEdit);
    setStateVariable("STUDENT_SETSTATE", "uploadData", null);
    setStateVariable("STUDENT_SETSTATE", "isDocumentModalOpen", true);
  };

  handleDeleteDocument = (docToDelete: any) => {
    const { student, setStateVariable } = this.props;

    const updatedDocs = (student.uploadedDocuments || []).map((doc: any) => {
      if (doc.documentId === docToDelete.documentId) {
        return { ...doc, indicator: 'D' };
      }
      return doc;
    });

    setStateVariable("STUDENT_SETSTATE", "uploadedDocuments", updatedDocs);
  };

  handleCancelEditDocument = () => {
    const { setStateVariable } = this.props;
    this.resetDocumentForm(setStateVariable);
    setStateVariable("STUDENT_SETSTATE", "isDocumentModalOpen", false);
  };

  resetDocumentForm = (setStateVariable: any) => {
    setStateVariable("STUDENT_SETSTATE", "documentName", "");
    setStateVariable("STUDENT_SETSTATE", "documentType", "");
    setStateVariable("STUDENT_SETSTATE", "fileName", "");
    setStateVariable("STUDENT_SETSTATE", "originalFileName", "");
    setStateVariable("STUDENT_SETSTATE", "uploadData", null);
    setStateVariable("STUDENT_SETSTATE", "responseFileName", "");
    setStateVariable("STUDENT_SETSTATE", "editingDocument", null);
    setStateVariable("STUDENT_SETSTATE", "previousFileName", "");
  };

  handleDownloadStudentFile = (fileName: string) => {
    const { downloadStudentFile } = this.props;
    downloadStudentFile(fileName);
  };


  populateStudentGrid = (student: any) => {
    try {
      const studentList = student.allStudentList ?? [];
      const filteredStudent = studentList;
      const studentGridColumnsDef: GridColDef[] = [
        {
          field: "editaction",
          headerName: "",
          align: "center",
          flex: 0.5,
          renderCell: (params: GridRenderCellParams) => {
            return (
              <Grid>
                <IconButton
                  aria-label="edit"
                  onClick={() => this.handleEditStudent(params.row)}
                >
                  <EditIcon fontSize="small" />
                </IconButton>

                <IconButton
                  aria-label="delete"
                  onClick={() => this.handleDeleteStudent(params.row)}
                >
                  <DeleteForeverIcon fontSize="small" />
                </IconButton>
              </Grid>
            );
          },
          sortable: false,
          filterable: false,
        },
        {
          field: "name",
          headerName: "Name",
          flex: 1.5,
          align: "left",
          headerAlign: "left",
          valueGetter: (params) => `${params.row.name}`,
        },
        {
          field: "code",
          headerName: "Student Code",
          flex: 1,
          align: "left",
          headerAlign: "left",
        },
        {
          field: "dob",
          headerName: "Date of Birth",
          flex: 1.5,
          align: "left",
          headerAlign: "left",
        },
        {
          field: "age",
          headerName: "Age",
          flex: 1,
          align: "left",
          headerAlign: "left",
        },
        {
          field: "email",
          headerName: "E-mail",
          flex: 1.5,
          align: "left",
          headerAlign: "left",
        },

        {
          field: "mobile",
          headerName: "Mobile No.",
          flex: 1,
          align: "left",
          headerAlign: "left",
        },
        {
          field: "stateName",
          headerName: "State",
          flex: 1,
          align: "left",
          headerAlign: "left"
        },
        {
          field: "cityName",
          headerName: "City",
          flex: 1,
          align: "left",
          headerAlign: "left"
        },
        {
          field: "isActive",
          headerName: "Active",
          flex: 1,
          align: "center",
          headerAlign: "center",
          renderCell: (params: GridRenderCellParams) => {
            return params.value ? (
              <DoneIcon fontSize="small" />
            ) : (
              ""
            );
          },
        },
      ];

      return (
        <div className="container">
          <Grid
            container
            style={{
              display: "flex",
              justifyContent: "center",
              height: window.innerHeight - 135,
              width: window.innerWidth - 60,
            }}
          >
            <DataGridPro
              className="studentPageGrid"
              columns={studentGridColumnsDef}
              getRowId={(row) => row.studentId}
              headerHeight={40}
              hideFooter
              rowHeight={35}
              rows={filteredStudent ?? []}
              style={{ width: "100%" }}
              sx={{
                '& .MuiDataGrid-columnHeaders': {
                  backgroundColor: 'lightblue',
                },
              }}
            />
          </Grid>
        </div>
      );
    } catch (error) {
      console.error("Student Page > populateStudentGrid :" + error);
    }
  };

  render() {
    const { student } = this.props;
    return (
      <div>
        <Backdrop
          open={student.isLoading}
          sx={{ color: '#fff', zIndex: (theme) => theme.zIndex.modal + 1 }}
        >
          <CircularProgress color="inherit" />
        </Backdrop>

        <Box
          sx={{
            border: '1px solid #ccc',
            borderRadius: 1,
            padding: 1,
          }}
        >
          <Grid container spacing={2}>
            <Grid className="left-align" item xs={2}>
              <TextField
                InputLabelProps={{ shrink: true }}
                id="outlined-basic"
                label="Search"
                onChange={this.handleSearchChange}
                placeholder="Name, State, City"
                size="small"
                value={student.searchText}
                variant="outlined"
              />
            </Grid>

            <Grid item xs={2}>
              <Box sx={{ position: 'relative', display: 'inline-block', width: '100%' }}>
                <Box
                  sx={{
                    position: 'absolute',
                    top: -8,
                    left: 10,
                    backgroundColor: 'white',
                    px: 0.5,
                    fontSize: '0.75rem',
                    color: '#5A5A5A',
                  }}
                >
                  Include
                </Box>

                <Box
                  sx={{
                    border: '1px solid #ccc',
                    borderRadius: 1,
                    padding: 1,
                    display: 'flex',
                    alignItems: 'center',
                    height: 21,
                    backgroundColor: '#fff',
                  }}
                >
                  <FormControlLabel
                    control={
                      <Checkbox
                        checked={student.showInactive}
                        name="showInactive"
                        onChange={this.handleShowInactive}
                        size="small"
                      />
                    }
                    label="Show Inactive"
                  />
                </Box>
              </Box>
            </Grid>

            <Grid item xs={6}>
              <h1 className="header-lbl">&nbsp;</h1>
            </Grid>

            <Grid className="right-align" item xs={2}>
              <Button
                className="header-btn"
                onClick={this.handleStudentAddBtn}
                size="small"
                sx={{ textTransform: "none" }}
                variant="contained"
              >
                {student.studentAddBtnTxt}
              </Button>
            </Grid>
          </Grid>

          {this.populateStudentGrid(student)}
        </Box>

        <StudentPopup
          onAddDocument={this.handleAddDocument}
          onAddSubject={this.handleAddSubject}
          onCancelEditDocument={this.handleCancelEditDocument}
          onCancelEditSubject={this.handleCancelEditSubject}
          onDeleteDocument={this.handleDeleteDocument}
          onDeleteSubject={this.handleDeleteSubject}
          onEditDocument={this.handleEditDocument}
          onEditSubject={this.handleEditSubject}
          onHandleDownloadStudentFile={this.handleDownloadStudentFile}
          onHandleStudentPopupCloseBtn={this.handleStudentPopupCloseBtn}
          onHandleStudentPopupSaveBtn={this.handleStudentPopupSaveBtn}
          onHandleTxtChange={this.handleTxtChange}
          onUpdateSubject={this.handleUpdateSubject}
        />

        <ConfirmationPopup
          confirmationPopupHeading={student.confirmationPopupHeading}
          confirmationPopupMessage={student.confirmationPopupMessage}
          confirmationPopupNoBtnTxt={student.confirmationPopupNoBtnTxt}
          confirmationPopupYesBtnTxt={student.confirmationPopupYesBtnTxt}
          isConfirmationPopupOpen={student.isConfirmationPopupOpen}
          onHandleConfirmationPopupNoBtn={this.handleConfirmationPopupNoBtn}
          onHandleConfirmationPopupYesBtn={this.handleConfirmationPopupYesBtn}
        />

        <ErrorPopup
          errorPopupHeading={student.errorPopupHeading}
          errorPopupMessage={student.errorPopupMessage}
          errorPopupOkBtnTxt={student.errorPopupOkBtnTxt}
          isErrorPopupOpen={student.isErrorPopupOpen}
          onHandleErrorPopupOkBtn={this.handleErrorPopupOkBtn}
        />

        <SuccessPopup
          isSuccessPopupOpen={student.isSuccessPopupOpen}
          onHandleSuccessPopupOkBtn={this.handleSuccessPopupOkBtn}
          successPopupHeading={student.successPopupHeading}
          successPopupMessage={student.successPopupMessage}
          successPopupOkBtnTxt={student.successPopupOkBtnTxt}
        />

        <WarningPopup
          isWarningPopupOpen={student.isWarningPopupOpen}
          onHandleWarningPopupOkBtn={this.handleWarningPopupOkBtn}
          warningPopupHeading={student.warningPopupHeading}
          warningPopupMessage={student.warningPopupMessage}
          warningPopupOkBtnTxt={student.warningPopupOkBtnTxt}
        />
      </div>
    );
  }
}

const mapStateToProps = (state: any) => {
  const app = state.app;
  const student = state.student;
  return {
    app: app,
    student: student,
  };
};

const mapDispatchToProps = {
  setStateVariable,
  updateStateVariables,
  getStudentList,
  getStudentDetails,
  updateStudent,
  uploadDocument,
  downloadStudentFile
};

const connector = connect(mapStateToProps, mapDispatchToProps);

type PropsFromRedux = ConnectedProps<typeof connector>;

type Props = PropsFromRedux;

export default connector(StudentPage);