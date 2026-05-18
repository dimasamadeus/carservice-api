#!/bin/sh
# Запусти один раз из корня проекта в Git Bash:
# sh INSTALL_HOOK.sh

cp hooks/pre-commit .git/hooks/pre-commit
chmod +x .git/hooks/pre-commit
echo "✅ Git hook установлен!"
