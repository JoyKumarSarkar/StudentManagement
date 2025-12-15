import { APP_SETSTATE } from "./AppActionType";

const initialState = {
    appTitle: "ADA Redux App",
    holidayMenuTxt: "Holiday",
    postingMenuTxt: "Posting",
    isShowBackdrop: false,
};

export function AppReducer(state = initialState, action: any) {
    switch (action.type) {
        case APP_SETSTATE:
            return { ...state, [action.payload.key]: action.payload.value };
        default:
            return state;
    }
}
