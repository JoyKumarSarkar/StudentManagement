import React from "react";
import { Button, Dialog, DialogActions, DialogContent, DialogTitle, Grid } from "@mui/material";
import HTMLReactParser from "html-react-parser";

class WarningPopup extends React.Component<Props> {

    shouldComponentUpdate() {
        return true;
    }

    render() {
        const { warningPopupHeading, warningPopupMessage, warningPopupOkBtnTxt, onHandleWarningPopupOkBtn, isWarningPopupOpen } = this.props;

        return (
            <Dialog open={isWarningPopupOpen}>
                <DialogTitle>{warningPopupHeading}</DialogTitle>

                <DialogContent>
                    <Grid container>
                        <Grid className="textalignCenter" item xs={12}>
                            {HTMLReactParser(warningPopupMessage === undefined || warningPopupMessage === null
                                ? ""
                                : warningPopupMessage)}
                        </Grid>
                    </Grid>
                </DialogContent>

                <DialogActions>
                    <Button className="commonPageBtnCustomized" color="primary" onClick={onHandleWarningPopupOkBtn} size="small" variant="contained">{warningPopupOkBtnTxt}</Button>
                </DialogActions>
            </Dialog>
        );
    }
}

type Props = {
    readonly isWarningPopupOpen: any;
    readonly onHandleWarningPopupOkBtn: any;
    readonly warningPopupHeading: any;
    readonly warningPopupMessage: any;
    readonly warningPopupOkBtnTxt: any;
};

export default WarningPopup;