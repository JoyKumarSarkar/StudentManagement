import { configureStore } from '@reduxjs/toolkit';
import { AppReducer } from '../AppReducer';
import { StudentReducer } from '../Components/Student/StudentReducer';

export const store = configureStore({
  reducer: {
    app: AppReducer,
    student : StudentReducer,
  },
});
