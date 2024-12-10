# limux amd image build (for most cases, excluding macOS)
docker buildx build --platform linux/amd64 -t noncommunicado/unistream-test-4:latest .
# ИЛИ
# platform specific image build 
docker build -t unistream-test-4:latest .