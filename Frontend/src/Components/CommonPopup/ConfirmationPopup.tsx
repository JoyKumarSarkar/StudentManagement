import React from "react";
import { Button, Dialog, DialogActions, DialogContent, DialogTitle, Grid } from "@mui/material";
import HTMLReactParser from "html-react-parser";

class ConfirmationPopup extends React.Component<Props> {

    shouldComponentUpdate() {
        return true;
    }

    render() {
        const { confirmationPopupHeading, confirmationPopupMessage, confirmationPopupNoBtnTxt, confirmationPopupYesBtnTxt, onHandleConfirmationPopupNoBtn, onHandleConfirmationPopupYesBtn, isConfirmationPopupOpen } = this.props;

        return (
            <Dialog open={isConfirmationPopupOpen}>
                <DialogTitle>{confirmationPopupHeading}</DialogTitle>

                <DialogContent>
                    <Grid container>
                        <Grid className="textalignCenter" item xs={12}>
                            {HTMLReactParser(confirmationPopupMessage === undefined || confirmationPopupMessage === null
                                ? ""
                                : confirmationPopupMessage)}
                        </Grid>
                    </Grid>
                </DialogContent>

                <DialogActions>
                    <Button className="commonPageBtnCustomized" color="primary" onClick={onHandleConfirmationPopupNoBtn} variant="outlined" size="small">{confirmationPopupNoBtnTxt}</Button>

                    <Button className="commonPageBtnCustomized" color="primary" onClick={onHandleConfirmationPopupYesBtn} variant="contained" size="small">{confirmationPopupYesBtnTxt}</Button>
                </DialogActions>
            </Dialog>
        );
    }
}

type Props = {
    readonly confirmationPopupHeading: string;
    readonly confirmationPopupMessage: string;
    readonly confirmationPopupNoBtnTxt: string;
    readonly confirmationPopupYesBtnTxt: string;
    readonly isConfirmationPopupOpen: boolean;
    readonly onHandleConfirmationPopupNoBtn: any;
    readonly onHandleConfirmationPopupYesBtn: any;
};

export default ConfirmationPopup;