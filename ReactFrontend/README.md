# ToDo React Frontend

Ky është frontend-i React për aplikacionin ToDo që komunikon me API-n .NET.

## Instalimi

1. **Instalo Node.js** (nëse nuk e ke):
   - Shkarko nga: https://nodejs.org/
   - Instalo versionin LTS

2. **Instalo dependencies**:
   ```bash
   npm install
   ```

3. **Starto aplikacionin**:
   ```bash
   npm start
   ```

Aplikacioni do të hapet në http://localhost:3000

## Përdorimi

1. **Regjistrohu** me një llogari të re
2. **Hyr** në sistem
3. **Krijo, edito, fshi** detyra
4. **Shëno detyrat si të përfunduara**

## API Endpoints

- `POST /api/auth/register` - Regjistrim
- `POST /api/auth/login` - Hyrje
- `GET /api/task` - Merr të gjitha detyrat
- `POST /api/task` - Krijo detyrë të re
- `PUT /api/task/{id}` - Edito detyrë
- `DELETE /api/task/{id}` - Fshi detyrë
- `POST /api/task/{id}/complete` - Shëno si të përfunduar

## Struktura e Projektit

- `src/components/` - Komponentët React
- `src/services/` - Shërbimet për API calls
- `src/contexts/` - Context për state management
- `src/pages/` - Faqet e aplikacionit
