import React from "react";
import { Button, Dialog, DialogActions, DialogContent, DialogTitle, Grid } from "@mui/material";
import HTMLReactParser from "html-react-parser";

class ErrorPopup extends React.Component<Props> {

    shouldComponentUpdate() {
        return true;
    }

    render() {
        const { errorPopupHeading, errorPopupMessage, errorPopupOkBtnTxt, onHandleErrorPopupOkBtn, isErrorPopupOpen } = this.props;

        return (
            <Dialog open={isErrorPopupOpen}>
                <DialogTitle>{errorPopupHeading}</DialogTitle>

                <DialogContent>
                    <Grid container>
                        <Grid className="textalignCenter" item xs={12}>
                            {HTMLReactParser(errorPopupMessage === undefined || errorPopupMessage === null
                                ? ""
                                : errorPopupMessage)}
                        </Grid>
                    </Grid>
                </DialogContent>

                <DialogActions>
                    <Button className="commonPageBtnCustomized" color="primary" onClick={onHandleErrorPopupOkBtn} size="small" variant="contained">{errorPopupOkBtnTxt}</Button>
                </DialogActions>
            </Dialog>
        );
    }
}

type Props = {
    readonly errorPopupHeading: any;
    readonly errorPopupMessage: any;
    readonly errorPopupOkBtnTxt: any;
    readonly isErrorPopupOpen: any;
    readonly onHandleErrorPopupOkBtn: any;
};

export default ErrorPopup;