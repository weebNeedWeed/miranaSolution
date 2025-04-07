const useBaseUrl = () => {
  const baseUrl = import.meta.env.VITE_BASE_ADDRESS;

  return baseUrl;
};

export { useBaseUrl };
