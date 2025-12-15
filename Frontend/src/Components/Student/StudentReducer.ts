import {
  STUDENT_ADD,
  STUDENT_DELETE,
  STUDENT_DOWNLOAD_FILE,
  STUDENT_DOWNLOAD_FILE_ERROR,
  STUDENT_GETDETAILS,
  STUDENT_GETLIST,
  STUDENT_POPUP_ADD,
  STUDENT_POPUP_CLOSE,
  STUDENT_POPUP_DELETE,
  STUDENT_POPUP_EDIT,
  STUDENT_SEARCH,
  STUDENT_SETSTATE,
  STUDENT_UPDATE,
  STUDENT_UPLOAD_DOCUMENT,
} from "./StudentActionType";

const initialState: any = {
  studentPageHeading: "Student List",
  studentAddBtnTxt: "+Student",
  StudentList: [],
  stateList: [],
  cities: [],
  responseIsSuccess: false,
  responseMessage: "",
  searchText: "",
  showInactive: false,

  isStudentPopupOpen: false,
  studentPopupHeading: "Student Details",
  studentPopupSaveBtnTxt: "Save",
  studentPopupCancelBtnTxt: "Cancel",
  indicator: "I",
  studentId: 0,
  firstName: "",
  lastName: "",
  code: "",
  dob: "",
  age: "",
  email: "",
  mobile: "",
  stateName: "",
  cityName: "",
  isActive: false,

  isConfirmationPopupOpen: false,
  confirmationPopupHeading: "Confirmation",
  confirmationPopupMessage: "",
  confirmationPopupYesBtnTxt: "Yes",
  confirmationPopupNoBtnTxt: "No",

  isInfoPopupOpen: false,
  infoPopupHeading: "Info",
  infoPopupMessage: "",
  infoPopupOkBtnTxt: "Ok",

  isSuccessPopupOpen: false,
  successPopupHeading: "Success",
  successPopupMessage: "",
  successPopupOkBtnTxt: "Ok",

  isErrorPopupOpen: false,
  errorPopupHeading: "Error",
  errorPopupMessage: "",
  errorPopupOkBtnTxt: "Ok",

  isWarningPopupOpen: false,
  warningPopupHeading: "Warning",
  warningPopupMessage: "",
  warningPopupOkBtnTxt: "OK",
  allStudentList: [],
  allStateList: [],
  allCityList: [],
  allSubjectWiseMarksList: [],
  subjectMarksList: [],
  subjectId: "",
  marks: "",
  editingSubject: null,
  isLoading: false,
  activeTabIndex: 0,
  documentsList: [],
  studentDocuments: [],
  documentName: "",
  documentType: "",
  originalFileName: "",
  previousFileName: "",
  documentFile: null,
  editingDocument: null,
  uploadData: null,
  uploadedDocuments: [],
  documentId: "",
  responseFileName: "",
  documentSize: "",
  isDocumentFormVisible: false,
  isDocumentModalOpen: false,
};

export function StudentReducer(state = initialState, action: any) {

  switch (action.type) {
    case STUDENT_SETSTATE:
      return {
        ...state,
        [action.payload.key]: action.payload.value
      };

    case STUDENT_ADD:
      return {
        ...state,
        responseIsSuccess: action.payload.isSuccess,
        responseMessage: action.payload.message,
        isStudentPopupOpen: action.payload.isSuccess
          ? false
          : state.isStudentPopupOpen,
        isSuccessPopupOpen: action.payload.isSuccess
          ? true
          : state.isSuccessPopupOpen,
        successPopupMessage: action.payload.isSuccess
          ? action.payload.message
          : state.successPopupMessage,
        isWarningPopupOpen: !action.payload.isSuccess
          ? true
          : state.isWarningPopupOpen,
        warningPopupMessage: !action.payload.isSuccess
          ? action.payload.message
          : state.warningPopupMessage,
      };

    case STUDENT_UPDATE:
      return {
        ...state,
        responseIsSuccess: action.payload.isSuccess,
        responseMessage: action.payload.message,
        isStudentPopupOpen: action.payload.isSuccess
          ? false
          : state.isStudentPopupOpen,
        isSuccessPopupOpen: action.payload.isSuccess
          ? true
          : state.isSuccessPopupOpen,
        successPopupMessage: action.payload.isSuccess
          ? action.payload.message
          : state.successPopupMessage,
        isWarningPopupOpen: !action.payload.isSuccess
          ? true
          : state.isWarningPopupOpen,
        warningPopupMessage: !action.payload.isSuccess
          ? action.payload.message
          : state.warningPopupMessage,
        subjectMarksList: action.payload.subjectWiseMarks || [],
      };

    case STUDENT_DELETE:
      return {
        ...state,
        responseIsSuccess: action.payload.isSuccess,
        responseMessage: action.payload.message,
        isConfirmationPopupOpen: action.payload.isSuccess
          ? false
          : state.isConfirmationPopupOpen,
        isSuccessPopupOpen: action.payload.isSuccess
          ? true
          : state.isSuccessPopupOpen,
        successPopupMessage: action.payload.isSuccess
          ? action.payload.message
          : state.successPopupMessage,
        isWarningPopupOpen: !action.payload.isSuccess
          ? true
          : state.isWarningPopupOpen,
        warningPopupMessage: !action.payload.isSuccess
          ? action.payload.message
          : state.warningPopupMessage,
      };

    case STUDENT_POPUP_ADD:
        return {
          ...state,
          isStudentPopupOpen: true,
          indicator: "I",
          studentId: 0,
          firstName: "",
          lastName: "",
          code: action.payload.code,
          dob: "",
          age: "",
          email: "",
          mobile: "",
          stateId: "",
          cityId: "",
          stateName: "",
          cityName: "",
          isActive: true,
          subjectMarksList: [],
          subjectId: "",
          marks: "",
          editingSubject: null,
        };

    case STUDENT_POPUP_DELETE:
      return {
        ...state,
        isConfirmationPopupOpen: true,
        confirmationPopupHeading: "Delete Confirmation",
        confirmationPopupMessage:
          "Do you want to delete <b>" + action.payload.firstName + " " + action.payload.lastName + " ?</b>",
        indicator: "D",
        studentId: action.payload.studentId,
      };

    case STUDENT_POPUP_CLOSE:
      return {
        ...state,
        isStudentPopupOpen: false,
        indicator: "I",
        studentId: 0,
        firstName: "",
        lastName: "",
        code: "",
        dob: "",
        age: "",
        email: "",
        mobile: "",
        stateName: "",
        cityName: "",
        isActive: "",
        subjectMarksList: [],
        subjectId: "",
        marks: "",
        editingSubject: null,
        documentName: "",
        documentType: "",
        originalFileName: "",
        uploadData: null,
        fileName: "",
        documentSize: "",
        responseFileName: "",
      };

    case STUDENT_SEARCH:
      return {
        ...state,
        searchText: action.payload,
      };

    case STUDENT_GETLIST:
      return {
        ...state,
        allStudentList: action.payload.studentList,
      };

    case STUDENT_GETDETAILS:
      return {
        ...state,
        allStateList: action.payload.stateList,
        allCityList: action.payload.cityList,
        allSubjectWiseMarksList: action.payload.subjectList,
        subjectMarksList: action.payload.subjectWiseMarks || [],
        uploadedDocuments: action.payload.studentWiseDocuments || [],
        documentSize: action.payload.documentSize,
      };

    case "STUDENT_SET_LOADING":
      return {
        ...state,
        isLoading: action.payload,
      };

    case STUDENT_UPLOAD_DOCUMENT:
      return {
        ...state,
        responseIsSuccess: action.payload.isSuccess,
        responseMessage: action.payload.message,
        isSuccessPopupOpen: action.payload.isSuccess
          ? true
          : state.isSuccessPopupOpen,
        successPopupMessage: action.payload.isSuccess
          ? action.payload.message
          : state.successPopupMessage,
        isWarningPopupOpen: !action.payload.isSuccess
          ? true
          : state.isWarningPopupOpen,
        warningPopupMessage: !action.payload.isSuccess
          ? action.payload.message
          : state.warningPopupMessage,
        documentName: action.payload.isSuccess ? "" : state.documentName,
        documentType: action.payload.isSuccess ? "" : state.documentType,
        uploadData: action.payload.isSuccess ? null : state.uploadData,
        responseFileName: action.payload.fileName,
        originalFileName: action.payload.originalFileName,
        documentSize: action.payload.documentSize,
      };
    case STUDENT_DOWNLOAD_FILE:
      return {
        ...state,
        downloadSuccess: true,
        lastDownloadedFile: action.payload,
      };

    case STUDENT_DOWNLOAD_FILE_ERROR:
      return {
        ...state,
        downloadSuccess: false,
        downloadError: action.payload,
      };

    default:
      return state;
  }
}
