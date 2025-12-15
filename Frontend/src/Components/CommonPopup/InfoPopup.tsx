import React from "react";
import { Button, Dialog, DialogActions, DialogContent, DialogTitle, Grid } from "@mui/material";
import HTMLReactParser from "html-react-parser";

class InfoPopup extends React.Component<Props> {

    shouldComponentUpdate() {
        return true;
    }

    render() {
        const { infoPopupHeading, infoPopupMessage, infoPopupOkBtnTxt, onHandleInfoPopupOkBtn, isInfoPopupOpen } = this.props;

        return (
            <Dialog open={isInfoPopupOpen}>
                <DialogTitle>{infoPopupHeading}</DialogTitle>

                <DialogContent>
                    <Grid container>
                        <Grid className="textalignCenter" item xs={12}>
                            {HTMLReactParser(infoPopupMessage === undefined || infoPopupMessage === null
                                ? ""
                                : infoPopupMessage)}
                        </Grid>
                    </Grid>
                </DialogContent>

                <DialogActions>
                    <Button className="commonPageBtnCustomized" color="primary" onClick={onHandleInfoPopupOkBtn} size="small" variant="contained">{infoPopupOkBtnTxt}</Button>
                </DialogActions>
            </Dialog>
        );
    }
}

type Props = {
    readonly infoPopupHeading: string;
    readonly infoPopupMessage: string;
    readonly infoPopupOkBtnTxt: string;
    readonly isInfoPopupOpen:  boolean;
    readonly onHandleInfoPopupOkBtn: any;
};

export default InfoPopup;