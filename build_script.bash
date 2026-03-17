#!/bin/bash

# ==============================================
# Скрипт для сборки Unity проекта (с устаревшими ключами)
# Использование: ./build_unity.sh <путь_к_проекту> [путь_к_редактору] [путь_для_билдов]
# ==============================================

set -e

PROJECT_PATH="${1:?Ошибка: не указан путь к проекту}"
DEFAULT_UNITY="$HOME/Unity/Hub/Editor/6000.3.10f1/Editor/Unity"
UNITY_PATH="${2:-$DEFAULT_UNITY}"
BUILD_PATH="${3:-}"

if [ ! -f "$UNITY_PATH" ]; then
    echo -e "\033[0;31mОшибка: файл Unity не найден по пути: $UNITY_PATH\033[0m"
    exit 1
fi

if [ -z "$BUILD_PATH" ]; then
    BUILD_PATH="$PROJECT_PATH/Builds"
fi

BUILD_PATH="$(realpath -m "$BUILD_PATH")"
LOG_DIR="$BUILD_PATH/Logs"

mkdir -p "$LOG_DIR"
mkdir -p "$BUILD_PATH/Windows"
mkdir -p "$BUILD_PATH/Linux"
mkdir -p "$BUILD_PATH/Android"

RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m'

echo -e "${YELLOW}Начинаем сборку проекта: $PROJECT_PATH${NC}"
echo "Редактор Unity: $UNITY_PATH"
echo "Билды будут сохранены в: $BUILD_PATH"

build_platform() {
    local platform_key=$1    # Ключ для командной строки
    local output_path=$2
    local log_file="$LOG_DIR/build_${platform_key}.log"

    echo -e "${YELLOW}Сборка для платформы: $platform_key${NC}"
    echo "Лог: $log_file"

    "$UNITY_PATH" \
        -batchmode \
        -quit \
        -projectPath "$PROJECT_PATH" \
        "$platform_key" "$output_path" \
        -logFile "$log_file"

    local exit_code=$?
    if [ $exit_code -eq 0 ]; then
        echo -e "${GREEN}✓ Сборка завершена успешно${NC}"
    else
        echo -e "${RED}✗ Ошибка при сборке (код $exit_code)${NC}"
        echo "Последние строки лога:"
        tail -20 "$log_file"
        exit $exit_code
    fi
}

# Используем устаревшие ключи
build_platform "-buildWindowsPlayer" "$BUILD_PATH/Windows/Game.exe"
build_platform "-buildLinux64Player" "$BUILD_PATH/Linux/Game.x86_64"
build_platform "-buildAndroid"       "$BUILD_PATH/Android/Game.apk"

echo -e "${GREEN}Все сборки успешно завершены!${NC}"
echo "Бинарные файлы находятся в: $BUILD_PATH"