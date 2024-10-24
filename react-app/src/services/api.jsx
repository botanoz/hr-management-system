import axios from 'axios';

const API_URL = import.meta.env.VITE_API_URL || 'https://localhost:7002/api';

const api = axios.create({
  baseURL: API_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('accessToken');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);

api.interceptors.response.use(
  (response) => response,
  async (error) => {
    if (error.response && error.response.status === 401) {
      localStorage.removeItem('accessToken');
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);

// Account
export const login = (email, password) => api.post('/account/login', { email, password });
export const register = (userData) => api.post('/account/register', userData);
export const logout = () => api.post('/account/logout');
export const getProfile = () => api.get('/account/profile');
export const updateProfile = (profileData) => api.put('/account/profile', profileData);
export const changePassword = (passwordData) => api.post('/account/change-password', passwordData);

// Company
export const getCompanies = () => api.get('/company');
export const getCompany = (id) => api.get(`/company/${id}`);
export const createCompany = (companyData) => api.post('/company', companyData);
export const updateCompany = (id, companyData) => api.put(`/company/${id}`, companyData);
export const deleteCompany = (id) => api.delete(`/company/${id}`);
export const getCurrentCompany = () => api.get('/company/current');

// Employee
export const getEmployees = () => api.get('/employee');
export const getEmployee = (id) => api.get(`/employee/${id}`);
export const createEmployee = (employeeData) => api.post('/employee', employeeData);
export const updateEmployee = (id, employeeData) => api.put(`/employee/${id}`, employeeData);
export const deleteEmployee = (id) => api.delete(`/employee/${id}`);
export const getCurrentEmployee = () => api.get('/employee/current');

// Leave
export const getLeaves = () => api.get('/leave');
export const getLeave = (id) => api.get(`/leave/${id}`);
export const createLeave = (leaveData) => api.post('/leave', leaveData);
export const updateLeave = (id, leaveData) => api.put(`/leave/${id}`, leaveData);
export const deleteLeave = (id) => api.delete(`/leave/${id}`);
export const approveLeave = (id, approvalData) => api.post(`/leave/${id}/approve`, approvalData);
export const rejectLeave = (id, rejectionData) => api.post(`/leave/${id}/reject`, rejectionData);

// Expense
export const getExpenses = () => api.get('/expense');
export const getExpense = (id) => api.get(`/expense/${id}`);
export const createExpense = (expenseData) => api.post('/expense', expenseData);
export const updateExpense = (id, expenseData) => api.put(`/expense/${id}`, expenseData);
export const deleteExpense = (id) => api.delete(`/expense/${id}`);
export const approveExpense = (id, approvalData) => api.post(`/expense/${id}/approve`, approvalData);
export const rejectExpense = (id, rejectionData) => api.post(`/expense/${id}/reject`, rejectionData);

// Shift
export const getShifts = () => api.get('/shift');
export const getShift = (id) => api.get(`/shift/${id}`);
export const createShift = (shiftData) => api.post('/shift', shiftData);
export const updateShift = (id, shiftData) => api.put(`/shift/${id}`, shiftData);
export const deleteShift = (id) => api.delete(`/shift/${id}`);

// Holiday
export const getHolidays = () => api.get('/holiday/company');
export const getHoliday = (id) => api.get(`/holiday/${id}`);
export const createHoliday = (holidayData) => api.post('/holiday', holidayData);
export const updateHoliday = (id, holidayData) => api.put(`/holiday/${id}`, holidayData);
export const deleteHoliday = (id) => api.delete(`/holiday/${id}`);

// Notification
export const getNotifications = () => api.get('/notification');
export const getNotification = (id) => api.get(`/notification/${id}`);
export const createNotification = (notificationData) => api.post('/notification', notificationData);
export const updateNotification = (id, notificationData) => api.put(`/notification/${id}`, notificationData);
export const deleteNotification = (id) => api.delete(`/notification/${id}`);
export const markNotificationAsRead = (id) => api.put(`/notification/${id}/read`);

// Report
export const getEmployeeReport = (companyId) => api.get('/report/employee', { params: { companyId } });
export const getLeaveReport = (companyId, startDate, endDate) => 
  api.get('/report/leave', { params: { companyId, startDate, endDate } });
export const getExpenseReport = (companyId, startDate, endDate) => 
  api.get('/report/expense', { params: { companyId, startDate, endDate } });
export const getShiftReport = (companyId, startDate, endDate) => 
  api.get('/report/shift', { params: { companyId, startDate, endDate } });
export const getCompanyReport = (companyId, startDate, endDate) => 
  api.get('/report/company', { params: { companyId, startDate, endDate } });

// Resume
export const getMyResume = () => api.get('/resume');
export const getResume = (employeeId) => api.get(`/resume/${employeeId}`);
export const createEducation = (employeeId, educationData) => 
  api.post(`/resume/${employeeId}/education`, educationData);
export const updateEducation = (employeeId, educationId, educationData) => 
  api.put(`/resume/${employeeId}/education/${educationId}`, educationData);
export const deleteEducation = (employeeId, educationId) => 
  api.delete(`/resume/${employeeId}/education/${educationId}`);

export const createWorkExperience = (employeeId, workExperienceData) => 
  api.post(`/resume/${employeeId}/work-experience`, workExperienceData);
export const updateWorkExperience = (employeeId, experienceId, workExperienceData) => 
  api.put(`/resume/${employeeId}/work-experience/${experienceId}`, workExperienceData);
export const deleteWorkExperience = (employeeId, experienceId) => 
  api.delete(`/resume/${employeeId}/work-experience/${experienceId}`);

export const createSkill = (employeeId, skillData) => 
  api.post(`/resume/${employeeId}/skill`, skillData);
export const updateSkill = (employeeId, skillId, skillData) => 
  api.put(`/resume/${employeeId}/skill/${skillId}`, skillData);
export const deleteSkill = (employeeId, skillId) => 
  api.delete(`/resume/${employeeId}/skill/${skillId}`);

export const createCertification = (employeeId, certificationData) => 
  api.post(`/resume/${employeeId}/certification`, certificationData);
export const updateCertification = (employeeId, certificationId, certificationData) => 
  api.put(`/resume/${employeeId}/certification/${certificationId}`, certificationData);
export const deleteCertification = (employeeId, certificationId) => 
  api.delete(`/resume/${employeeId}/certification/${certificationId}`);

export const createLanguage = (employeeId, languageData) => 
  api.post(`/resume/${employeeId}/language`, languageData);
export const updateLanguage = (employeeId, languageId, languageData) => 
  api.put(`/resume/${employeeId}/language/${languageId}`, languageData);
export const deleteLanguage = (employeeId, languageId) => 
  api.delete(`/resume/${employeeId}/language/${languageId}`);

export const updateAdditionalInformation = (employeeId, additionalInfo) => 
  api.put(`/resume/${employeeId}/additional-info`, additionalInfo);

// Dashboard
export const getDashboardData = () => api.get('/dashboard');
export const getEmployeeSummary = () => api.get('/dashboard/employee-summary');
export const getCompanyOverview = () => api.get('/dashboard/company-overview');
export const getDashboardNotifications = () => api.get('/dashboard/notifications');
export const getUpcomingEvents = () => api.get('/dashboard/upcoming-events');
export const getPendingApprovals = () => api.get('/dashboard/pending-approvals');

export default api;
