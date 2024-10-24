import React, { createContext, useReducer, useEffect, useMemo, useCallback } from 'react';
import PropTypes from 'prop-types';
import { jwtDecode } from 'jwt-decode';
import { useNavigate } from 'react-router-dom';

import { login as apiLogin, logout as apiLogout, getProfile, getCurrentCompany } from '../services/api';

const initialState = {
  isAuthenticated: false,
  isInitialized: false,
  isLoading: true,
  user: null,
  company: null,
};

const handlers = {
  INITIALIZE: (state, action) => {
    const { isAuthenticated, user, company } = action.payload;
    return {
      ...state,
      isAuthenticated,
      isInitialized: true,
      isLoading: false,
      user,
      company,
    };
  },
  LOGIN: (state, action) => {
    const { user, company } = action.payload;
    return {
      ...state,
      isAuthenticated: true,
      isLoading: false,
      user,
      company,
    };
  },
  LOGOUT: (state) => ({
    ...state,
    isAuthenticated: false,
    isLoading: false,
    user: null,
    company: null,
  }),
  UPDATE_USER: (state, action) => ({
    ...state,
    user: action.payload,
  }),
  UPDATE_COMPANY: (state, action) => ({
    ...state,
    company: action.payload,
  }),
};

const reducer = (state, action) =>
  handlers[action.type] ? handlers[action.type](state, action) : state;

const AuthContext = createContext({
  ...initialState,
  method: 'jwt',
  login: () => Promise.resolve(),
  logout: () => Promise.resolve(),
  updateUser: () => Promise.resolve(),
  updateCompany: () => Promise.resolve(),
});

const setSession = (accessToken) => {
  if (accessToken) {
    localStorage.setItem('accessToken', accessToken);
  } else {
    localStorage.removeItem('accessToken');
  }
};

const verifyToken = (token) => {
  if (!token) return false;
  try {
    const decoded = jwtDecode(token);
    const currentTime = Date.now() / 1000;
    return decoded.exp > currentTime;
  } catch (error) {
    return false;
  }
};

export const AuthProvider = ({ children }) => {
  const [state, dispatch] = useReducer(reducer, initialState);
  const navigate = useNavigate();

  useEffect(() => {
    const initialize = async () => {
      try {
        const accessToken = localStorage.getItem('accessToken');

        if (accessToken && verifyToken(accessToken)) {
          setSession(accessToken);
          const userResponse = await getProfile();
          const companyResponse = await getCurrentCompany();

          dispatch({
            type: 'INITIALIZE',
            payload: {
              isAuthenticated: true,
              user: userResponse.data,
              company: companyResponse.data,
            },
          });
        } else {
          dispatch({
            type: 'INITIALIZE',
            payload: {
              isAuthenticated: false,
              user: null,
              company: null,
            },
          });
        }
      } catch (err) {
        console.error('Failed to initialize auth', err);
        dispatch({
          type: 'INITIALIZE',
          payload: {
            isAuthenticated: false,
            user: null,
            company: null,
          },
        });
      }
    };

    initialize();
  }, []);

  const login = useCallback(async (email, password) => {
    const response = await apiLogin(email, password);
    const { token, user } = response.data;

    setSession(token);
    const companyResponse = await getCurrentCompany();

    dispatch({
      type: 'LOGIN',
      payload: {
        user,
        company: companyResponse.data,
      },
    });

    navigate('/dashboard');
  }, [navigate]);

  const logout = useCallback(async () => {
    try {
      await apiLogout();
    } catch (error) {
      console.error('Logout failed', error);
    } finally {
      setSession(null);
      dispatch({ type: 'LOGOUT' });
      navigate('/login');
    }
  }, [navigate]);

  const updateUser = useCallback(async () => {
    try {
      const response = await getProfile();
      dispatch({
        type: 'UPDATE_USER',
        payload: response.data,
      });
    } catch (error) {
      console.error('Failed to update user', error);
    }
  }, []);

  const updateCompany = useCallback(async () => {
    try {
      const response = await getCurrentCompany();
      dispatch({
        type: 'UPDATE_COMPANY',
        payload: response.data,
      });
    } catch (error) {
      console.error('Failed to update company', error);
    }
  }, []);

  const contextValue = useMemo(
    () => ({
      ...state,
      method: 'jwt',
      login,
      logout,
      updateUser,
      updateCompany,
    }),
    [state, login, logout, updateUser, updateCompany]
  );

  return <AuthContext.Provider value={contextValue}>{children}</AuthContext.Provider>;
};

AuthProvider.propTypes = {
  children: PropTypes.node.isRequired,
};

export default AuthContext;