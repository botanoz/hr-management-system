import { useNavigate } from 'react-router-dom';

const useRouter = () => {
  const navigate = useNavigate();

  return {
    back: () => navigate(-1),
    push: (path) => navigate(path),
    replace: (path) => navigate(path, { replace: true }),
  };
};

export default useRouter;