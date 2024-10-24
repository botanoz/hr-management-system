import { GlobalStyles as MUIGlobalStyles } from '@mui/material';

const GlobalStyles = () => (
  <MUIGlobalStyles
    styles={{
      '*': {
        margin: 0,
        padding: 0,
        boxSizing: 'border-box',
      },
      html: {
        width: '100%',
        height: '100%',
        WebkitOverflowScrolling: 'touch',
      },
      body: {
        width: '100%',
        height: '100%',
        backgroundColor: (theme) => theme.palette.background.default,
      },
      '#root': {
        width: '100%',
        height: '100%',
        display: 'flex',
        flexDirection: 'column',
      },
      input: {
        '&[type=number]': {
          MozAppearance: 'textfield',
          '&::-webkit-outer-spin-button': {
            margin: 0,
            WebkitAppearance: 'none',
          },
          '&::-webkit-inner-spin-button': {
            margin: 0,
            WebkitAppearance: 'none',
          },
        },
      },
      img: {
        display: 'block',
        maxWidth: '100%',
      },
      ul: {
        margin: 0,
        padding: 0,
      },
    }}
  />
);

export default GlobalStyles;
