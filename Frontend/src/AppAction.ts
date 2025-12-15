export const setStateVariable =
  (actionType: string, key: string, newValue: any) => (dispatch: any) => {
    dispatch({
      type: actionType,
      payload: {
        key: key,
        value: newValue,
      },
    });
    return Promise.resolve();
  };

export const updateStateVariables =
  (actionType: string, payload?: any) => (dispatch: any) => {
    dispatch({
      type: actionType,
      payload: payload,
    });
    return Promise.resolve();
  };