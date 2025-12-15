import { Grid } from "@mui/material";
import React from "react";
import { connect, ConnectedProps } from "react-redux";
import { withRouter } from "react-router-dom";
import { updateStateVariables } from "../AppAction";

class HeaderPage extends React.Component<Props> {

    componentDidMount() {
        const { history } = this.props;
        if (history.location.pathname === "/") {
            history.push('/student');
        }
    }

    shouldComponentUpdate() {
        return true;
    }

    render() {
        const { student } = this.props;
        return (
            <Grid container>
                <Grid item sx={{  mb: 1 }} xs={12}>
                    <h1 className="header-lbl">{student.studentPageHeading}</h1>
                </Grid>
            </Grid>
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
    updateStateVariables,
};

const connector = connect(mapStateToProps, mapDispatchToProps);

type PropsFromRedux = ConnectedProps<typeof connector>;

type Props = PropsFromRedux & {
    readonly history: any;
};

export default withRouter(connector(HeaderPage));