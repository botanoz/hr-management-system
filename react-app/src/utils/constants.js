export const USER_ROLES = {
    ADMIN: 'admin',
    MANAGER: 'manager',
    EMPLOYEE: 'employee',
  };
  
  export const LEAVE_TYPES = {
    VACATION: 'vacation',
    SICK: 'sick',
    PERSONAL: 'personal',
    MATERNITY: 'maternity',
    PATERNITY: 'paternity',
  };
  
  export const EXPENSE_CATEGORIES = {
    TRAVEL: 'travel',
    EQUIPMENT: 'equipment',
    SUPPLIES: 'supplies',
    MEALS: 'meals',
    OTHER: 'other',
  };
  
  export const SHIFT_TYPES = {
    MORNING: 'morning',
    AFTERNOON: 'afternoon',
    NIGHT: 'night',
  };
  
  export const COMPANY_TYPES = {
    CORPORATION: 'corporation',
    LLC: 'llc',
    PARTNERSHIP: 'partnership',
    SOLE_PROPRIETORSHIP: 'sole_proprietorship',
  };
  
  export const API_ENDPOINTS = {
    LOGIN: '/auth/login',
    LOGOUT: '/auth/logout',
    PROFILE: '/auth/profile',
    EMPLOYEES: '/employees',
    LEAVES: '/leaves',
    EXPENSES: '/expenses',
    SHIFTS: '/shifts',
    COMPANIES: '/companies',
  };
  
  export const ROUTES = {
    HOME: '/',
    LOGIN: '/login',
    DASHBOARD: '/dashboard',
    EMPLOYEES: '/employees',
    LEAVES: '/leaves',
    EXPENSES: '/expenses',
    SHIFTS: '/shifts',
    COMPANIES: '/companies',
    PROFILE: '/profile',
    SETTINGS: '/settings',
  };
  
  export const HOLIDAY_TYPES = {
    PUBLIC: 'public',
    COMPANY: 'company',
    OPTIONAL: 'optional',
  };