import React, { useState } from 'react';
import PropTypes from 'prop-types';
import { NavLink as RouterLink } from 'react-router-dom';
import { Box, List, ListItemText, Collapse } from '@mui/material';
import { alpha } from '@mui/material/styles';
import { StyledNavItem, StyledNavItemIcon } from './styles';
import useAuth from '../../hooks/useAuth';
import { hasPermission } from '../../utils/permissions';

const NavSection = ({ data = [], ...other }) => {
  const { user } = useAuth();

  return (
    <Box
      {...other}
      sx={{
        backgroundColor: (theme) => alpha(theme.palette.background.default, 0.88),
        borderRadius: (theme) => theme.shape.borderRadius * 2,
        padding: (theme) => theme.spacing(2, 1),
      }}
    >
      <List disablePadding>
        {data.map((item) => (
          <NavItem key={item.title} item={item} user={user} />
        ))}
      </List>
    </Box>
  );
};

NavSection.propTypes = {
  data: PropTypes.array,
};

const NavItem = ({ item, user }) => {
  const { title, path, icon, info, children, requiredPermission } = item;
  const [open, setOpen] = useState(false);

  if (requiredPermission && !hasPermission(user.role, requiredPermission)) {
    return null;
  }

  const handleClick = () => {
    setOpen(!open);
  };

  return (
    <>
      <StyledNavItem
        component={children ? 'div' : RouterLink}
        to={path}
        onClick={children ? handleClick : undefined}
      >
        <StyledNavItemIcon>{icon && icon}</StyledNavItemIcon>
        <ListItemText 
          primary={title} 
          primaryTypographyProps={{ 
            variant: 'subtitle2',
            sx: { fontWeight: 'fontWeightMedium' }
          }} 
        />
        {info && info}
      </StyledNavItem>

      {children && (
        <Collapse in={open} timeout="auto" unmountOnExit>
          <List component="div" disablePadding sx={{ pl: 3 }}>
            {children.map((child) => (
              <NavItem key={child.title} item={child} user={user} />
            ))}
          </List>
        </Collapse>
      )}
    </>
  );
};

NavItem.propTypes = {
  item: PropTypes.object,
  user: PropTypes.object,
};

export default NavSection;