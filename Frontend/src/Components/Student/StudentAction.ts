import { STUDENT_ADD, STUDENT_DELETE, STUDENT_DOWNLOAD_FILE, STUDENT_DOWNLOAD_FILE_ERROR, STUDENT_GETDETAILS, STUDENT_GETLIST, STUDENT_SET_LOADING, STUDENT_UPDATE, STUDENT_UPLOAD_DOCUMENT } from "./StudentActionType";

const appConfig = window as any;

const BASE_API_URL =
  appConfig.BASE_API_URL.charAt(appConfig.BASE_API_URL.length - 1) === "/"
    ? appConfig.BASE_API_URL.substring(0, appConfig.BASE_API_URL.length - 1)
    : appConfig.BASE_API_URL;

const BasicAuthentication = appConfig.BasicAuthentication;

const getAuthorizedHeaders = {
  accept: "application/json",
  Authorization: "Basic " + btoa(BasicAuthentication),
};

const postAuthorizedHeaders = {
  "Content-Type": "application/json",
  Authorization: "Basic " + btoa(BasicAuthentication)
};

export const getStudentList = (param: any) => async (dispatch: any) => {
  dispatch({
    type: STUDENT_SET_LOADING,
    payload: true,
  })

  try {
    const response = await fetch(
      BASE_API_URL + "/Student/GetStudentList" + "?searchText=" + param.searchText + "&showInactive=" + param.showInactive,
      {
        method: "GET",
        headers: getAuthorizedHeaders,
      }
    );
    const data = await response.json();
    return dispatch({
      type: STUDENT_GETLIST,
      payload: data,
    });
  } catch (error) {
    console.error("StudentAction.ts > getStudentList:" + error);
  }
  finally {
    dispatch({
      type: STUDENT_SET_LOADING,
      payload: false,
    })
  }
};

export const getStudentDetails = (param: any) => async (dispatch: any) => {
  dispatch({
    type: STUDENT_SET_LOADING,
    payload: true,
  })
  try {
    const response = await fetch(
      BASE_API_URL + "/Student/GetStudentDetails/" + param.id,
      {
        method: "GET",
        headers: getAuthorizedHeaders,
      }
    );
    const data = await response.json();
    return dispatch({
      type: STUDENT_GETDETAILS,
      payload: data,
    });
  } catch (error) {
    console.error("StudentAction.ts > getStudentDetails:" + error);
  }
  finally {
    dispatch({
      type: STUDENT_SET_LOADING,
      payload: false,
    })
  }
};

export const updateStudent = (param: any) => async (dispatch: any) => {
  dispatch({
    type: STUDENT_SET_LOADING,
    payload: true,
  })
  try {
    const response = await fetch(
      BASE_API_URL + "/Student/UpdateStudent",
      {
        method: "POST",
        headers: postAuthorizedHeaders,
        body: JSON.stringify(param)
      }
    );
    const data = await response.json();
    const actionType = (param.indicator === "I") ? STUDENT_ADD : ((param.indicator === "U") ? STUDENT_UPDATE : STUDENT_DELETE);
    return dispatch({
      type: actionType,
      payload: data
    });
  } catch (err) {
    console.error("Error in StudentAction.ts > updateStudent:" + err);
  } finally {
    dispatch({
      type: STUDENT_SET_LOADING,
      payload: false,
    })
  }
};

export const uploadDocument = (param: FormData) => async (dispatch: any) => {
  try {
    const response = await fetch(BASE_API_URL + "/Student/UploadDocument", {
      method: "POST",
      headers: {
        Authorization: "Basic " + btoa(BasicAuthentication),
      },
      body: param,
    });

    const data = await response.json();

    return dispatch({
      type: STUDENT_UPLOAD_DOCUMENT,
      payload: data,
    });
  } catch (err) {
    console.error("Error in StudentAction.ts > uploadDocument:", err);
  }
};


// export const downloadStudentFile = (fileName: string) => async (dispatch: any) => {
//   try {
//     const response = await fetch(`${BASE_API_URL}/Student/DownloadDocument/${fileName}`, {
//       method: "GET",
//       headers: getAuthorizedHeaders, 
//     });

//     if (!response.ok) {
//       throw new Error("File not found or download failed");
//     }

//     const blob = await response.blob();
//     const url = window.URL.createObjectURL(blob);

//     const a = document.createElement("a");
//     a.href = url;
//     a.download = fileName;
//     document.body.appendChild(a);
//     a.click();
//     a.remove();
//     window.URL.revokeObjectURL(url);

//     dispatch({
//       type: STUDENT_DOWNLOAD_FILE,
//       payload: fileName,
//     });
//   } catch (error: any) {
//     dispatch({
//       type: STUDENT_DOWNLOAD_FILE_ERROR,
//       payload: error.message || "Download failed",
//     });
//   }
// };



export const downloadStudentFile = (fileName: string) => async (dispatch: any) => {
  try {
    const response = await fetch(BASE_API_URL + "/Student/DownloadDocument/" + fileName, {
      method: "GET",
      headers: getAuthorizedHeaders,
    });

    if (!response.ok) {
      throw new Error("Download failed");
    }

    const blob = await response.blob();
    const url = window.URL.createObjectURL(blob);
    const a = document.createElement("a");
    a.href = url;
    a.download = fileName;
    a.click();
    window.URL.revokeObjectURL(url);
    console.log (blob);

    dispatch({
      type: STUDENT_DOWNLOAD_FILE,
      payload: fileName,
    }); 
  } catch (error: any) {
    dispatch({
      type: STUDENT_DOWNLOAD_FILE_ERROR,
      payload: error.message || "Download failed",
    });
  }
};
