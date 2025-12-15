import Backdrop from '@mui/material/Backdrop';
import CircularProgress from '@mui/material/CircularProgress';
import { LicenseInfo } from '@mui/x-data-grid-pro';
import React from 'react';
import { connect, ConnectedProps } from 'react-redux';
import { Route, Switch } from 'react-router-dom';
import './App.css';
import HeaderPage from './Components/HeaderPage';
import StudentPage from './Components/Student/StudentPage';

LicenseInfo.setLicenseKey(
  'e711be1ae5c2f17995619ed4b49b8791Tz00NTc0MyxFPTE2ODY5MTU3ODEyNTcsUz1wcm8sTE09cGVycGV0dWFsLEtWPTI='
);

class App extends React.Component<Props> {

  componentDidMount() {
    const { app } = this.props;
    document.title = app.appTitle;
  }

  shouldComponentUpdate() {
    return true;
  }

  render() {
    const { app } = this.props;

    const routes = (
      <Switch>
        <Route component={StudentPage} path='/student' />
      </Switch>
    );

    return (
      <div
        className="App" style={{
          border: '1px solid #ccc',
          borderRadius: 4,
          padding: 3,
          margin: 5,
        }}
      >
        <HeaderPage />

        <Backdrop open={app.isShowBackdrop} sx={{ color: '#fff', zIndex: (theme) => theme.zIndex.drawer + 1 }}>
          <CircularProgress color="inherit" />
        </Backdrop>

        <header className="App-header">
          {routes}
        </header>
      </div>
    );
  }
}

const mapStateToProps = (state: any) => {
  const app = state.app;
  return {
    app: app,
  };
};

const connector = connect(mapStateToProps);

type PropsFromRedux = ConnectedProps<typeof connector>;

type Props = PropsFromRedux;

export default connector(App);