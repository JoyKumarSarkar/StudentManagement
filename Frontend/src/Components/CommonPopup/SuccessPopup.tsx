import React from "react";
import { Button, Dialog, DialogActions, DialogContent, DialogTitle, Grid } from "@mui/material";
import HTMLReactParser from "html-react-parser";

class SuccessPopup extends React.Component<Props> {

    shouldComponentUpdate() {
        return true;
    }

    render() {
        const { successPopupHeading, successPopupMessage, successPopupOkBtnTxt, onHandleSuccessPopupOkBtn, isSuccessPopupOpen } = this.props;

        return (
            <Dialog open={isSuccessPopupOpen}>
                <DialogTitle>{successPopupHeading}</DialogTitle>

                <DialogContent>
                    <Grid container>
                        <Grid className="textalignCenter" item xs={12}>
                            {HTMLReactParser(successPopupMessage === undefined || successPopupMessage === null
                                ? ""
                                : successPopupMessage)}
                        </Grid>
                    </Grid>
                </DialogContent>

                <DialogActions>
                    <Button className="commonPageBtnCustomized" color="primary" onClick={onHandleSuccessPopupOkBtn} size="small" variant="contained">{successPopupOkBtnTxt}</Button>
                </DialogActions>
            </Dialog>
        );
    }
}

type Props = {
    readonly isSuccessPopupOpen: boolean;
    readonly onHandleSuccessPopupOkBtn: any;
    readonly successPopupHeading: string;
    readonly successPopupMessage: string;
    readonly successPopupOkBtnTxt: string;
};

export default SuccessPopup;