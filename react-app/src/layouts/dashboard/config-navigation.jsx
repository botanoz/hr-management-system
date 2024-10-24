import DashboardIcon from '@mui/icons-material/Dashboard';
import PeopleIcon from '@mui/icons-material/People';
import CalendarTodayIcon from '@mui/icons-material/CalendarToday';
import ShoppingCartIcon from '@mui/icons-material/ShoppingCart';
import AccessTimeIcon from '@mui/icons-material/AccessTime';
import BusinessIcon from '@mui/icons-material/Business';
import AccountCircleIcon from '@mui/icons-material/AccountCircle';
import HolidayIcon from '@mui/icons-material/BeachAccess'; // Assuming an icon for holidays

const navConfig = [
  {
    title: 'Gösterge Paneli', // 'Dashboard' translated to 'Gösterge Paneli'
    path: '/dashboard',
    icon: <DashboardIcon />,
    roles: ['Admin', 'Manager', 'Employee'],
  },
  {
    title: 'Çalışanlar', // 'Employees' translated to 'Çalışanlar'
    path: '/employees',
    icon: <PeopleIcon />,
    roles: ['Admin', 'Manager'],
  },
  {
    title: 'İzinler', // 'Leaves' translated to 'İzinler'
    path: '/leaves',
    icon: <CalendarTodayIcon />,
    roles: ['Admin', 'Manager', 'Employee'],
  },
  {
    title: 'Giderler', // 'Expenses' translated to 'Giderler'
    path: '/expenses',
    icon: <ShoppingCartIcon />,
    roles: ['Admin', 'Manager', 'Employee'],
  },
  {
    title: 'Vardiyalar', // 'Shifts' translated to 'Vardiyalar'
    path: '/shifts',
    icon: <AccessTimeIcon />,
    roles: ['Admin', 'Manager', 'Employee'],
  },
  {
    title: 'Şirketler', // 'Companies' translated to 'Şirketler'
    path: '/companies',
    icon: <BusinessIcon />,
    roles: ['Admin'],
  },
  {
    title: 'Tatil Günleri', // 'Holidays' translated to 'Tatil Günleri'
    path: '/holidays',
    icon: <HolidayIcon />,
    roles: ['Admin', 'Manager', 'Employee'],
  },
  {
    title: 'Profil', // 'Profile' translated to 'Profil'
    path: '/profile',
    icon: <AccountCircleIcon />,
    roles: ['Admin', 'Manager', 'Employee'],
  }
];

export default navConfig;
