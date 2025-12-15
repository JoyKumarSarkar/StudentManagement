/* eslint-disable react/jsx-no-literals */
/* eslint-disable react/require-optimization */
import React from "react";
import {
    Box, Button, Checkbox, Dialog, DialogActions, DialogContent, DialogTitle, FormControlLabel, Grid, Tab, Tabs, TextField, IconButton, Typography, Autocomplete
} from "@mui/material";
import CloseIcon from "@mui/icons-material/Close";
import DeleteForeverIcon from "@mui/icons-material/DeleteForever";
import EditIcon from "@mui/icons-material/Edit";
import { connect, ConnectedProps } from "react-redux";
import { DataGridPro, GridColDef } from '@mui/x-data-grid-pro';
import { STUDENT_SETSTATE } from "./StudentActionType";
import { setStateVariable } from "../../AppAction";
import CloudUploadIcon from '@mui/icons-material/CloudUpload';

class StudentPopup extends React.Component<Props> {

    private fileInputRef = React.createRef<HTMLInputElement>();

    render() {
        const {
            student, onHandleTxtChange, onHandleStudentPopupCloseBtn, onHandleStudentPopupSaveBtn, setStateVariable, onAddSubject, onEditSubject, onDeleteSubject, onUpdateSubject,
            onCancelEditSubject, onAddDocument, onEditDocument, onDeleteDocument, onCancelEditDocument, onHandleDownloadStudentFile } = this.props;

        const filteredCities = student.allCityList.filter((city: any) => city.stateId === student.stateId);

        const subjectColumns: GridColDef[] = [
            {
                field: 'actions',
                headerName: '',
                flex: 0.5,
                sortable: false,
                filterable: false,
                disableColumnMenu: true,
                renderCell: (params) => (
                    <>
                        <IconButton size="small" >
                            <EditIcon fontSize="small" onClick={() => onEditSubject(params.row)} />
                        </IconButton>

                        <IconButton size="small" >
                            <DeleteForeverIcon fontSize="small" onClick={() => onDeleteSubject(params.row)} />
                        </IconButton>
                    </>
                ),
            },
            { field: 'subjectName', headerName: 'Subject Name', flex: 1, align: "left", headerAlign: "left" },
            { field: 'marks', headerName: 'Marks', flex: 0.5, align: "left", headerAlign: "left" },
        ];

        const documentColumns: GridColDef[] = [

            {
                field: 'actions', headerName: '', sortable: false, filterable: false, disableColumnMenu: true, flex: 1, renderCell: (params) => (
                    <>
                        <IconButton size="small"> <EditIcon fontSize="small" onClick={() => onEditDocument(params.row)} /> </IconButton>

                        <IconButton size="small"> <DeleteForeverIcon fontSize="small" onClick={() => onDeleteDocument(params.row)} /> </IconButton>
                    </>
                ),
            },

            { align: "left", field: 'documentName', flex: 1.5, headerName: 'Document Name', headerAlign: "left" },
            { field: 'documentType', headerName: 'Document Type', flex: 1.5, align: "left", headerAlign: "left" },
            { field: 'fileName', headerName: 'File Name', flex: 1.5, align: "left", headerAlign: "left" },
            { field: 'originalFileName', headerName: 'Original FileName', flex: 1.5, align: "left", headerAlign: "left" },
            { field: 'documentSize', headerName: 'Size', flex: 1, align: 'left', headerAlign: 'left', valueFormatter: (params) => formatFileSize(params.value) },
            {
                field: 'viewFile', headerName: 'View File', sortable: false, filterable: false, disableColumnMenu: true, flex: 1, renderCell: (params) => (
                    <Button onClick={() => onHandleDownloadStudentFile(params.row.fileName)} size="small" sx={{ textTransform: "none" }} >
                        Download
                    </Button>
                ),
            },
        ];

        const formatFileSize = (bytes: number) => {
            if (!bytes) return '';
            const sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB'];
            let i = 0;
            let size = bytes;

            while (size >= 1024 && i < sizes.length - 1) {
                size /= 1024;
                i++;
            }

            return size.toFixed(2) + ' ' + sizes[i];
        };


        return (
            <Dialog open={student.isStudentPopupOpen} >
                <div className="container" style={{ backgroundColor: "lightblue", height: "33px" }}>
                    <DialogTitle sx={{ fontSize: 18, fontWeight: 'bold' }}>
                        {`${student.studentPopupHeading}${student.indicator === "U" ? ` [${student.firstName} ${student.lastName} - ${student.code}]` : ""}`}
                    </DialogTitle>

                    <IconButton onClick={onHandleStudentPopupCloseBtn}>
                        <CloseIcon fontSize="small" />
                    </IconButton>
                </div>

                <DialogContent sx={{ minHeight: 420 }}>
                    <Grid container spacing={2}>
                        <Grid item xs={4}>
                            <TextField fullWidth label="First Name" name="firstName" onChange={onHandleTxtChange} size="small" value={student.firstName} variant="outlined" />
                        </Grid>

                        <Grid item xs={4}>
                            <TextField fullWidth label="Last Name" name="lastName" onChange={onHandleTxtChange} size="small" value={student.lastName} variant="outlined" />
                        </Grid>

                        <Grid item xs={4}>
                            <TextField disabled fullWidth label="Student Code" name="code" onChange={onHandleTxtChange} size="small" type="text" value={student.code} variant="outlined" />
                        </Grid>

                        <Grid item xs={4}>
                            <TextField InputLabelProps={{ shrink: true }} fullWidth label="Date of Birth" name="dob" onChange={onHandleTxtChange} size="small" type="date" value={student.dob} variant="outlined" />
                        </Grid>

                        <Grid item xs={2}>
                            <TextField disabled fullWidth label="Age" name="age" onChange={onHandleTxtChange} size="small" type="number" value={student.age} variant="outlined" />
                        </Grid>

                        <Grid item xs={6}>
                            <TextField fullWidth label="E-mail" name="email" onChange={onHandleTxtChange} size="small" value={student.email} variant="outlined" />
                        </Grid>

                        <Grid item xs={4}>
                            <TextField fullWidth label="Mobile No." name="mobile" onChange={onHandleTxtChange} size="small" value={student.mobile} variant="outlined" />
                        </Grid>

                        <Grid item xs={4}>
                            <Autocomplete
                                fullWidth
                                getOptionLabel={(option) => option.stateName}
                                onChange={(_e, newValue) => {
                                    onHandleTxtChange({
                                        target: {
                                            name: "stateId",
                                            value: newValue ? newValue.stateId : "",
                                        },
                                    });
                                }}
                                options={student.allStateList}
                                renderInput={(params) => <TextField {...params} label="State" variant="outlined" />}
                                size="small"
                                value={student.allStateList.find((s: any) =>
                                    s.stateId === student.stateId) || null}
                            />
                        </Grid>

                        <Grid item xs={4}>
                            <Autocomplete
                                disabled={!student.stateId}
                                fullWidth
                                getOptionLabel={(option) => option.cityName}
                                onChange={(_e, newValue) => {
                                    onHandleTxtChange({
                                        target: {
                                            name: "cityId",
                                            value: newValue ? newValue.cityId : "",
                                        },
                                    });
                                }}
                                options={filteredCities}
                                renderInput={(params) => <TextField {...params} label="City" variant="outlined" />}
                                size="small"
                                value={filteredCities.find((c: any) => c.cityId === student.cityId) || null}
                            />
                        </Grid>

                        <Grid item xs={12}>
                            <Box sx={{ borderBottom: 1, borderColor: 'divider' }}>
                                <Tabs
                                    onChange={(e, newValue) =>
                                        setStateVariable(STUDENT_SETSTATE, "activeTabIndex", newValue)}
                                    value={student.activeTabIndex}
                                >
                                    <Tab label="Subject" sx={{ textTransform: "none" }} />

                                    <Tab label="Document" sx={{ textTransform: "none" }} />
                                </Tabs>
                            </Box>

                            <Box sx={{ minHeight: 210, maxHeight: 210, mt: 1 }}>
                                {student.activeTabIndex === 0 && (
                                    <Box sx={{ p: 2 }}>
                                        <Grid container spacing={2}>
                                            <Grid item xs={6}>
                                                <Box sx={{ mt: -1 }}>
                                                    <Typography fontSize={13} fontWeight={500} mb={0.5}>Subject</Typography>

                                                    <Autocomplete
                                                        fullWidth
                                                        getOptionLabel={(option) => option.subjectName}
                                                        onChange={(e, newValue) => {
                                                            onHandleTxtChange({ target: { name: "subjectId", value: newValue ? newValue.subjectId : "" } });
                                                        }}
                                                        options={student.allSubjectWiseMarksList}
                                                        renderInput={(params) => <TextField {...params} variant="outlined" />}
                                                        size="small"
                                                        value={student.allSubjectWiseMarksList.find((s: any) => s.subjectId === student.subjectId) || null}
                                                    />
                                                </Box>
                                            </Grid>

                                            <Grid item xs={3}>
                                                <Typography fontSize={13} fontWeight={500} mb={0.5}>Marks</Typography>

                                                <TextField
                                                    fullWidth
                                                    name="marks"
                                                    onChange={onHandleTxtChange}
                                                    size="small"
                                                    type="number"
                                                    value={student.marks}
                                                    variant="outlined"
                                                />
                                            </Grid>

                                            <Grid alignItems="flex-end" display="flex" item xs={3}>
                                                {student.editingSubject ? (
                                                    <>
                                                        <Button
                                                            onClick={onUpdateSubject}
                                                            size="small"
                                                            sx={{ textTransform: "none", mb: 0.8 }}
                                                            variant="contained"
                                                        >
                                                            Update
                                                        </Button>

                                                        <Button onClick={onCancelEditSubject} size="small" sx={{ textTransform: "none", ml: 1.1, mb: 0.8 }} variant="outlined">
                                                            Cancel
                                                        </Button>
                                                    </>
                                                ) : (
                                                    <Button color="primary" onClick={onAddSubject} size="small" sx={{ textTransform: "none", ml: 4, mb: 0.5 }} variant="contained">
                                                        Add
                                                    </Button>
                                                )}
                                            </Grid>

                                            <Grid item xs={12}>
                                                <Box sx={{ height: 120, width: '100%', overflowX: 'auto' }}>
                                                    <DataGridPro
                                                        autoHeight
                                                        columns={subjectColumns}
                                                        getRowId={(row) => row.marksId}
                                                        headerHeight={30}
                                                        hideFooter
                                                        rowHeight={30}
                                                        rows={(student.subjectMarksList || []).filter((s: any) => s.indicator !== "D")}
                                                        sx={{
                                                            '& .MuiDataGrid-columnHeaders': {
                                                                backgroundColor: 'lightblue',
                                                            },
                                                        }}
                                                    />
                                                </Box>
                                            </Grid>
                                        </Grid>
                                    </Box>
                                )}

                                {student.activeTabIndex === 1 && (
                                    <Box sx={{ p: 2 }}>
                                        <Box display="flex" justifyContent="flex-end" >
                                            <Button
                                                onClick={() => {
                                                    setStateVariable("STUDENT_SETSTATE", "isDocumentModalOpen", true);
                                                    setStateVariable("STUDENT_SETSTATE", "editingDocument", null);
                                                }}
                                                size="small"
                                                sx={{ textTransform: 'none' }}
                                                variant="contained"
                                            >
                                                + Add
                                            </Button>
                                        </Box>

                                        <Box sx={{ mt: 2, height: 120, width: '100%', overflowX: 'auto' }}>
                                            <Box sx={{ minWidth: 800 }}>
                                                <DataGridPro
                                                    autoHeight
                                                    columns={documentColumns}
                                                    getRowId={(row) => row.documentId}
                                                    headerHeight={30}
                                                    hideFooter
                                                    rowHeight={30}
                                                    rows={(student.uploadedDocuments || []).filter((doc: any) => doc.indicator !== 'D')}
                                                    sx={{
                                                        '& .MuiDataGrid-columnHeaders': {
                                                            backgroundColor: 'lightblue',
                                                        },
                                                    }}
                                                />
                                            </Box>
                                        </Box>

                                        <Dialog
                                            fullWidth
                                            maxWidth="sm"
                                            open={student.isDocumentModalOpen}
                                        >
                                            <div className="container" style={{ backgroundColor: "lightblue", height: "33px" }}>
                                                <DialogTitle sx={{ fontSize: 18, fontWeight: 'bold' }}>
                                                    {student.editingDocument ? "Edit Document" : "Add Document"}
                                                </DialogTitle>

                                                <IconButton onClick={onCancelEditDocument}>
                                                    <CloseIcon fontSize="small" />
                                                </IconButton>
                                            </div>

                                            <DialogContent dividers>
                                                <Grid container spacing={2}>
                                                    <Grid item xs={12}>
                                                        <Typography fontSize={13} fontWeight={500} mb={0.5}>Document Name</Typography>

                                                        <TextField
                                                            InputLabelProps={{ shrink: true }}
                                                            fullWidth
                                                            name="documentName"
                                                            onChange={onHandleTxtChange}
                                                            size="small"
                                                            value={student.documentName}
                                                            variant="outlined"
                                                        />
                                                    </Grid>

                                                    <Grid item xs={12}>
                                                        <Typography fontSize={13} fontWeight={500} mb={0.5}>Document Type</Typography>

                                                        <Autocomplete
                                                            fullWidth
                                                            onChange={(e, newValue) => {
                                                                setStateVariable("STUDENT_SETSTATE", "documentType", newValue || "");
                                                            }}
                                                            options={['Document Type 1', 'Document Type 2', 'Document Type 3']}
                                                            renderInput={(params) => <TextField {...params} variant="outlined" />}
                                                            size="small"
                                                            value={student.documentType || ''}
                                                        />
                                                    </Grid>

                                                    <Grid item xs={12}>
                                                        <Box
                                                            onClick={() => this.fileInputRef.current?.click()}
                                                            sx={{
                                                                border: '1px dashed #ccc',
                                                                borderRadius: '4px',
                                                                height: 100,
                                                                backgroundColor: 'rgb(234, 247, 253)',
                                                                display: 'flex',
                                                                alignItems: 'center',
                                                                justifyContent: 'center',
                                                                flexDirection: 'column',
                                                                cursor: 'pointer',
                                                                position: 'relative'
                                                            }}
                                                        >
                                                            <CloudUploadIcon sx={{ fontSize: 32 }} />

                                                            <Typography>{student.originalFileName ? student.originalFileName : "Upload File"}</Typography>

                                                            <input
                                                                hidden
                                                                onChange={(e) => {
                                                                    const fileObj = e.target.files?.[0];
                                                                    if (fileObj) {
                                                                        const param = new FormData();
                                                                        param.append("file", fileObj);
                                                                        setStateVariable("STUDENT_SETSTATE", "uploadData", param);
                                                                        setStateVariable("STUDENT_SETSTATE", "originalFileName", fileObj.name);
                                                                    }
                                                                }}
                                                                ref={this.fileInputRef}
                                                                type="file"
                                                            />
                                                        </Box>
                                                    </Grid>
                                                </Grid>
                                            </DialogContent>

                                            <DialogActions>
                                                <Button
                                                    onClick={onAddDocument}
                                                    size="small"
                                                    sx={{ textTransform: 'none' }}
                                                    variant="contained"
                                                >
                                                    {student.editingDocument ? "Update" : "Submit"}
                                                </Button>

                                                <Button
                                                    onClick={onCancelEditDocument}
                                                    size="small"
                                                    sx={{ textTransform: 'none' }}
                                                    variant="outlined"
                                                >
                                                    Cancel
                                                </Button>
                                            </DialogActions>
                                        </Dialog>
                                    </Box>
                                )}
                            </Box>
                        </Grid>
                    </Grid>
                </DialogContent>

                <DialogActions>
                    <Grid container spacing={2}>
                        <FormControlLabel
                            control={
                                <Checkbox
                                    checked={student.isActive}
                                    name="isActive"
                                    onChange={onHandleTxtChange}
                                    size="small"
                                />
                            }
                            label={<Typography fontSize="14px" fontWeight={500}>Active</Typography>}
                            labelPlacement="start"
                            sx={{ ml: 44, mt: 2 }}
                        />
                    </Grid>

                    <Button onClick={onHandleStudentPopupCloseBtn} size="small" sx={{ textTransform: "none" }} variant="outlined">
                        {student.studentPopupCancelBtnTxt}
                    </Button>

                    <Button onClick={onHandleStudentPopupSaveBtn} size="small" sx={{ textTransform: "none" }} variant="contained">
                        {student.studentPopupSaveBtnTxt}
                    </Button>
                </DialogActions>
            </Dialog>
        );
    }
}

const mapStateToProps = (state: any) => ({
    student: state.student,
});
const mapDispatchToProps = {
    setStateVariable,
};
const connector = connect(mapStateToProps, mapDispatchToProps);
type PropsFromRedux = ConnectedProps<typeof connector>;
type Props = PropsFromRedux & {
    readonly onHandleStudentPopupCloseBtn: any;
    readonly onHandleStudentPopupSaveBtn: any;
    readonly onHandleTxtChange: any;
    readonly onAddDocument: any;
    readonly onEditDocument: any;
    readonly onDeleteDocument: any;
    readonly onDeleteSubject: any;
    readonly onEditSubject: any;
    readonly onAddSubject: any;
    readonly onUpdateSubject: any;
    readonly onCancelEditSubject: any;
    readonly onCancelEditDocument: any;
    readonly onHandleDownloadStudentFile: any;

};

export default connector(StudentPopup);