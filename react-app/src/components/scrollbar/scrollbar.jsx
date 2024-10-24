import PropTypes from 'prop-types';
import { forwardRef } from 'react';
import { StyledScrollbar } from './styles';

const Scrollbar = forwardRef(({ children, sx, ...other }, ref) => {
  const userAgent = typeof navigator === 'undefined' ? 'SSR' : navigator.userAgent;
  const isMobile = /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(userAgent);

  if (isMobile) {
    return <div ref={ref} style={{ overflow: 'auto', ...sx }} {...other}>{children}</div>;
  }

  return (
    <StyledScrollbar ref={ref} sx={sx} {...other}>
      {children}
    </StyledScrollbar>
  );
});

Scrollbar.propTypes = {
  children: PropTypes.node.isRequired,
  sx: PropTypes.object,
};

export default Scrollbar;